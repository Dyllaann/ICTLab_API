using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using log4net;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Data;
using TimeTable2.Engine;
using TimeTable2.Repository;
using TimeTable2.Services;

namespace TimeTable2.Controllers
{
    [RoutePrefix("api/timetable")]
    public class TimeTableController : TimeTableApiController
    {
        [HttpPost]
        [SwaggerOperation("Find")]
        [Route("Find")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(bool))]
        public HttpResponseMessage Find(int start, int end, int dayofweek)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var classroomRepository = new ClassroomRepository(context);
            var service = new TimeTableService(classroomRepository);
                
            var emptyRooms = service.FindEmpty(start, end, dayofweek);
            return Request.CreateResponse(HttpStatusCode.OK, emptyRooms);
        }


        [HttpGet]
        [SwaggerOperation("scrape")]
        [Route("scrape")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<Classroom>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Scraping unable")]
        public HttpResponseMessage Scrape(int week)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var scraperRepository = new ScraperRepository(context);
            var classroomRepository = new ClassroomRepository(context);
            var bookingRepository = new BookingRepository(context);
            var classRepository = new ClassRepository(context);
            var scraperService = new ScraperService(scraperRepository, classroomRepository, classRepository, bookingRepository);

            Task.Run(() => scraperService.Scrape(week));

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [SwaggerOperation("Health")]
        [Route("Health")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public HttpResponseMessage Health()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [SwaggerOperation("Notify")]
        [Route("Notify")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> TestNotifcation()
        {
            var notifier = new Notifier.Notifier();
            await notifier.Notify("testUser", "Oi!", "hello matey", "API");
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
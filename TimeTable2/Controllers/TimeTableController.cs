using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using log4net;
using log4net.Repository.Hierarchy;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Data;
using TimeTable2.Engine;
using TimeTable2.Scraper;
using TimeTable2.Services;

namespace TimeTable2.Controllers
{
    [RoutePrefix("api/timetable")]
    public class TimeTableController : TimeTableApiController
    {
        [HttpPost]
        [SwaggerOperation("testAuthentication")]
        [Route("testAuthentication")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GoogleUserProfile))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public async Task<HttpResponseMessage> Authorize(string token)
        {
            var logger = LogManager.GetLogger("TimeTableController");
            var isAuthorized = await APISerivce.Authorize(token, logger);
            return (isAuthorized != null)
                ? Request.CreateResponse(HttpStatusCode.OK, isAuthorized)
                : Request.CreateResponse(HttpStatusCode.Unauthorized, "UnAuthorized");
        }

        [HttpPost]
        [SwaggerOperation("testAuthenticationCall")]
        [Route("testAuthenticationCall")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GoogleUserProfile))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public async Task<HttpResponseMessage> AuthorizeByCall()
        {
            var logger = LogManager.GetLogger("TimeTableController");
            var header = Request.Headers.Authorization;
            if (header == null)
            {
                logger.Info($"Empty Authorization header.");
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "UnAuthorized");
            }

            var token = header.Parameter;
            var isAuthorized = await APISerivce.Authorize(token, logger);
            return (isAuthorized != null)
                ? Request.CreateResponse(HttpStatusCode.OK, isAuthorized)
                : Request.CreateResponse(HttpStatusCode.Unauthorized, "UnAuthorized");
        }

        [HttpGet]
        [SwaggerOperation("testRoom")]
        [Route("testRoom")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Classroom))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public HttpResponseMessage TestRoom()
        {
            var classroom = new Classroom
            {
                Capacity = 30,
                Id = Guid.NewGuid(),
                Maintenance = MaintenanceStatus.OK,
                RoomId = "H4.318"
            };

            return Request.CreateResponse(HttpStatusCode.OK, classroom);
        }

        [HttpGet]
        [SwaggerOperation("scrape")]
        [Route("scrape")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<Classroom>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Scraping unable")]
        public HttpResponseMessage Scrape(int quarter, int week)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var repository = new ScraperRepository(context);
            var scraperService = new ScraperService(repository);
            var html = scraperService.Scrape(quarter, week);

            return Request.CreateResponse(HttpStatusCode.OK, html);
        }
    }
}
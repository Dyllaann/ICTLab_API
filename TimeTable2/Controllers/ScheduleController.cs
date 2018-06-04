using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Data;
using TimeTable2.Engine;
using TimeTable2.Repository;
using TimeTable2.Services;

namespace TimeTable2.Controllers
{

    [RoutePrefix("api/Schedule")]
    public class ScheduleController : TimeTableApiController
    {
        #region Lokaal
        [HttpGet]
        [SwaggerOperation("Lokaal/{roomCode}/{week}")]
        [Route("Lokaal/{roomCode}/{week}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<Course>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public HttpResponseMessage GetScheduleForRoom(string roomCode, int week)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var repository = new ClassroomRepository(context);
            var service = new TimeTableService(repository); 

            var room = service.GetClassroomScheduleByCodeAndWeek(roomCode, week);
                
            return Request.CreateResponse(HttpStatusCode.OK, room);
        }
        #endregion

        #region Class
        [HttpGet]
        [SwaggerOperation("Class/{classCode}/{week}")]
        [Route("Class/{classCode}/{week}")] 
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<Course>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public HttpResponseMessage GetScheduleForClass(string classCode, int week)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var repository = new ClassroomRepository(context);
            var service = new TimeTableService(repository);
    
            var room = service.GetClassScheduleByCodeAndWeek(classCode, week);
            
            return Request.CreateResponse(HttpStatusCode.OK, room);
        }
        #endregion

        #region Student / Teachers
        [HttpGet]
        [SwaggerOperation("Personal/Today")]
        [Route("Personal/Today")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Course))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public HttpResponseMessage GetPersonalScheduleToday()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Course());
        }


        #endregion
    }
}
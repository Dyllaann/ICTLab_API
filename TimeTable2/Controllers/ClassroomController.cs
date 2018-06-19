using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Data;
using TimeTable2.Engine;
using TimeTable2.Repository;
using TimeTable2.Services;

namespace TimeTable2.Controllers
{
    [RoutePrefix("api/Classroom")]
    public class ClassroomController : TimeTableApiController
    {
        [HttpGet]
        [SwaggerOperation("All")]
        [Route("All")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<Classroom>))]
        public HttpResponseMessage GetAllClassrooms()
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var repository = new ClassroomRepository(context);
            var service = new ClassroomService(repository);

            var rooms = service.GetAllClassrooms();

            return rooms == null
                ? Request.CreateResponse(HttpStatusCode.NoContent)
                : Request.CreateResponse(HttpStatusCode.OK, rooms);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Engine;
using TimeTable2.Models;
using TimeTable2.Services.Models;

namespace TimeTable2.Controllers
{
    [RoutePrefix("api/Room")]
    public class RoomController : TimeTableApiController
    {
        [HttpGet]
        [SwaggerOperation("CurrentAvailable/{id}")]
        [Route("CurrentAvailable/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<AvailableRoom>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Rooms were not found")]
        public HttpResponseMessage GetCurrentAvailable(DateTime start)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new OwClassroom());
        }
    }
}
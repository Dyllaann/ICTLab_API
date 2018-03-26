using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Models;
using TimeTable2.Services.Models;

namespace TimeTable2.Controllers
{

    [RoutePrefix("api/Schedule")]
    public class ScheduleController : ApiController
    {
        #region Room
        [HttpGet]
        [SwaggerOperation("Room/{roomCode}")]
        [Route("Room/{roomCode}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OwSchedule))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public HttpResponseMessage GetCurrentAvailable(Guid classroomId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new OwSchedule());
        }
        #endregion

        #region Student / Teachers
        [HttpGet]
        [SwaggerOperation("Personal/Today")]
        [Route("Personal/Today")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OwSchedule))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public HttpResponseMessage GetPersonalScheduleToday()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new OwSchedule());
        }


        #endregion
    }
}
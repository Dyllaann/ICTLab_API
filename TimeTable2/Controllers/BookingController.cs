using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Models;

namespace TimeTable2.Controllers
{
    [RoutePrefix("api/Booking")]
    public class BookingController : ApiController
    {
        [HttpPost]
        [SwaggerOperation("Filter")]
        [Route("Filter")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<AvailableRoom>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public HttpResponseMessage Filter(RoomFilterModel filter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new List<AvailableRoom>());
        }

        [HttpPost]
        [SwaggerOperation("Book")]
        [Route("Book")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(BookingResult))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public HttpResponseMessage Book(BookRoomModel bookingInfo)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new BookingResult());
        }
    }
}
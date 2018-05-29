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
    [RoutePrefix("api/Booking")]
    public class BookingController : TimeTableApiController
    {
        [HttpPost]
        [SwaggerOperation("Book")]
        [Route("Book")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(bool))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public HttpResponseMessage Book(Booking booking)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var bookingRepository = new BookingRepository(context);
            var classroomRepository = new ClassroomRepository(context);
            var service = new BookingService(bookingRepository, classroomRepository);

            booking.Owner = UserId;
            service.BookRoom(booking);

            return Request.CreateResponse(HttpStatusCode.OK, new Booking());
        }
    }
}
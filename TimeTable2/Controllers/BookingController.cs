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
using TimeTable2.Engine.Bookings;
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
        [SwaggerResponse(HttpStatusCode.PreconditionFailed, Description = "'The room was already booked'," +
                                                                          "'There was a normal lesson during the time you would like to reserve'," +
                                                                          "'There is maintenance in this classroom during the time you would like to reserve.'")]
        public HttpResponseMessage Book(Booking booking)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var bookingRepository = new BookingRepository(context);
            var classroomRepository = new ClassroomRepository(context);
            var service = new BookingService(bookingRepository, classroomRepository);

            booking.Owner = UserId;
            var bookingService = service.BookRoom(booking, UserProfile);
            switch (bookingService)
            {
                case BookingAvailability.Success:
                    return Request.CreateResponse(HttpStatusCode.OK, booking);
                case BookingAvailability.Booked:
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "The room was already booked");
                case BookingAvailability.Scheduled:
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "There was a normal lesson during the time you would like to reserve");
                case BookingAvailability.Maintenance:
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "There is maintenance in this classroom during the time you would like to reserve.");
                default:
                    return null;
            }
        }

        [HttpGet]
        [SwaggerOperation("Filter")]
        [Route("Filter/{guests}/{startBlock}/{endBlock}/{week}/{weekDay}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<FilterClassroom>))]
        public HttpResponseMessage Filter(int guests, int startBlock, int endBlock, int week, int weekDay)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var bookingRepository = new BookingRepository(context);
            var classroomRepository = new ClassroomRepository(context);
            var service = new BookingService(bookingRepository, classroomRepository);

            var bookingService = service.Filter(guests, startBlock, endBlock, week, weekDay);
            return Request.CreateResponse(HttpStatusCode.OK, bookingService);

        }

        [HttpGet]
        [SwaggerOperation("Filter")]
        [Route("Bookings/{roomCode}/{week}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<Booking>))]
        public HttpResponseMessage GetBookingsPerRoomPerWeek(string roomCode, int week)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var bookingRepository = new BookingRepository(context);
            var classroomRepository = new ClassroomRepository(context);
            var service = new BookingService(bookingRepository, classroomRepository);

            var bookings = service.GetBookingsPerRoomPerWeek(roomCode, week);
            return Request.CreateResponse(HttpStatusCode.OK, bookings);

        }
    }
}
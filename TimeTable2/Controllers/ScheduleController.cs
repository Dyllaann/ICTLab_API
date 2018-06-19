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
        [HttpGet]
        [SwaggerOperation("Classroom/{roomCode}/{week}")]
        [Route("Classroom/{roomCode}/{week}")]
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

        [HttpGet]
        [SwaggerOperation("MyBookings/{week}")]
        [Route("MyBookings/{week}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<Booking>))]
        public HttpResponseMessage GetBookingsForUser(int week)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var classroomRepository = new ClassroomRepository(context);
            var bookingRepository = new BookingRepository(context);
            var service = new BookingService(bookingRepository, classroomRepository);

            var bookings = service.GetBookingsForUser(week, UserId);

            return Request.CreateResponse(HttpStatusCode.OK, bookings);
        }

        [HttpGet]
        [SwaggerOperation("AvailableWeeks")]
        [Route("AvailableWeeks")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<int>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "No available weeks found")]
        public HttpResponseMessage GetAvailableWeeksHttpResponseMessage()
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var repository = new ClassroomRepository(context);
            var service = new TimeTableService(repository);

            var weeks = service.GetAvailableWeeks();

            return Request.CreateResponse(HttpStatusCode.OK, weeks);
        }

    }
}
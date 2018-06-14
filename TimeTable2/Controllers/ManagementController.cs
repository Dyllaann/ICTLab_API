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
    [RoutePrefix("api/Management")]
    public class ManagementController : TimeTableApiController
    {
        [HttpGet]
        [SwaggerOperation("AllUsers")]
        [Route("AllUsers")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetAllUsers()
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var repository = new UserRepository(context);
            var managementService = new ManagementService();
            var userService = new UserService(repository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var allUsers = managementService.GetAllUsers(repository);
            return Request.CreateResponse(HttpStatusCode.OK, allUsers);
        }

        [HttpPost]
        [SwaggerOperation("Book")]
        [Route("Book")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        public HttpResponseMessage BookMaintenance(Booking booking)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var userRepository = new UserRepository(context);
            var bookingRepository = new BookingRepository(context);
            var classroomRepository = new ClassroomRepository(context);

            var managementService = new ManagementService();
            var bookingService = new BookingService(bookingRepository, classroomRepository);
            var userService = new UserService(userRepository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var allUsers = managementService.MaintenanceBooking(classroomRepository, bookingRepository, userRepository, bookingService, booking, UserId);
            return Request.CreateResponse(HttpStatusCode.OK, allUsers);
        }

        [HttpDelete]
        [SwaggerOperation("Book")]
        [Route("Book")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public HttpResponseMessage DeleteMaintenance(Guid bookingId)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var userRepository = new UserRepository(context);
            var bookingRepository = new BookingRepository(context);
            var classroomRepository = new ClassroomRepository(context);

            var bookingService = new BookingService(bookingRepository, classroomRepository);
            var userService = new UserService(userRepository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var deletedBooking = bookingService.DeleteBooking(bookingId);
            if (deletedBooking)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
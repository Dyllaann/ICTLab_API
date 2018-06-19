using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Data;
using TimeTable2.Engine;
using TimeTable2.Engine.Management;
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
            if (user.Role != TimeTableRole.Management && user.Role != TimeTableRole.Admin)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var allUsers = managementService.GetAllUsers(repository);
            return Request.CreateResponse(HttpStatusCode.OK, allUsers);
        }

        [HttpGet]
        [SwaggerOperation("MaintenaceBookings/{week}")]
        [Route("MaintenaceBookings/{week}")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetAllMaintenaceBookingsByWeek(int week)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var repository = new UserRepository(context);
            var userService = new UserService(repository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management && user.Role != TimeTableRole.Admin && user.Role != TimeTableRole.Fit)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var bookingRepository = new BookingRepository(context);
            var classroomRepository = new ClassroomRepository(context);
            var bookingService = new BookingService(bookingRepository, classroomRepository);

            var allBookings = bookingService.GetAllMaintenanceBookings(week);
            return Request.CreateResponse(HttpStatusCode.OK, allBookings);
        }

        [HttpGet]
        [SwaggerOperation("AllMaintenaceBookings")]
        [Route("AllMaintenaceBookings")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        public HttpResponseMessage GetAllMaintenaceBookings()
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var repository = new UserRepository(context);
            var userService = new UserService(repository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management && user.Role != TimeTableRole.Admin && user.Role != TimeTableRole.Fit)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var bookingRepository = new BookingRepository(context);
            var classroomRepository = new ClassroomRepository(context);
            var bookingService = new BookingService(bookingRepository, classroomRepository);

            var allBookings = bookingService.GetAllMaintenanceBookings();
            return Request.CreateResponse(HttpStatusCode.OK, allBookings);
        }

        [HttpPost]
        [SwaggerOperation("Book")]
        [Route("Book")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        public async Task<HttpResponseMessage> BookMaintenance(Booking booking)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var userRepository = new UserRepository(context);
            var bookingRepository = new BookingRepository(context);
            var classroomRepository = new ClassroomRepository(context);

            var managementService = new ManagementService();
            var bookingService = new BookingService(bookingRepository, classroomRepository);
            var userService = new UserService(userRepository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management && user.Role != TimeTableRole.Admin && user.Role != TimeTableRole.Fit)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var notifier = new Notifier.Notifier();
            var allUsers = await managementService.MaintenanceBooking(classroomRepository, bookingRepository, userRepository, bookingService, booking, UserId, notifier);
            return Request.CreateResponse(HttpStatusCode.OK, allUsers);
        }

        [HttpPost]
        [SwaggerOperation("EditUser")]
        [Route("EditUser")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.PreconditionFailed, "You can't edit yourself")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        public HttpResponseMessage EditRol(UserEdit edit)
        {
            if (edit.UserId == UserId)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "You can't edit yourself");
            }

            var isDefined = Enum.IsDefined(typeof(TimeTableRole), edit.NewRole);
            if (!isDefined)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "New role is not a valid one");
            }

            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var userRepository = new UserRepository(context);

            var managementService = new ManagementService();
            var userService = new UserService(userRepository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Admin)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var isUserEdited = managementService.EditUser(userRepository, edit);
            return isUserEdited
                ? Request.CreateResponse(HttpStatusCode.OK)
                : Request.CreateResponse(HttpStatusCode.BadRequest);

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
            if (user.Role != TimeTableRole.Management && user.Role != TimeTableRole.Admin && user.Role != TimeTableRole.Fit)
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
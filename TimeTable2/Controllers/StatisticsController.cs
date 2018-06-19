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
using TimeTable2.Engine.Statistics;
using TimeTable2.Repository;
using TimeTable2.Services;

namespace TimeTable2.Controllers
{
    [RoutePrefix("api/Statistics")]
    public class StatisticsController : TimeTableApiController
    {   
        #region MostUsedClassroom
        [HttpGet]
        [SwaggerOperation("MostUsedClassroomsLessons/{week}/{top}")]
        [Route("MostUsedClassroomsLessons/{week}/{top}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<MostUsedRoom>))]
        public HttpResponseMessage MostUsedClassroomsLessons(int top, int week)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var userRepository = new UserRepository(context);
            var userService = new UserService(userRepository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management && user.Role != TimeTableRole.Admin && user.Role != TimeTableRole.Fit)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var repository = new ClassroomRepository(context);
            var service = new StatisticsService();

            var rooms = service.GetMostUsedClassroomsByLessons(repository, top, week);

            return Request.CreateResponse(HttpStatusCode.OK, rooms);
        }

        [HttpGet]
        [SwaggerOperation("MostUsedClassroomsBookings/{week}/{top}")]
        [Route("MostUsedClassroomsBookings/{week}/{top}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<MostUsedRoom>))]
        public HttpResponseMessage MostUsedClassroomsBookings(int top, int week)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var userRepository = new UserRepository(context);
            var userService = new UserService(userRepository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management && user.Role != TimeTableRole.Admin && user.Role != TimeTableRole.Fit)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var repository = new ClassroomRepository(context);
            var service = new StatisticsService();

            var rooms = service.GetMostUsedClassroomsByBookings(repository, top, week);

            return Request.CreateResponse(HttpStatusCode.OK, rooms);
        }
        #endregion

        #region AmountOfBookings
        [HttpGet]
        [SwaggerOperation("AmountOfMaintenanceBookings/{week}")]
        [Route("AmountOfMaintenanceBookings/{week}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(int))]
        public HttpResponseMessage AmountOfMaintenanceBookings(int week)
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var userRepository = new UserRepository(context);
            var userService = new UserService(userRepository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management && user.Role != TimeTableRole.Admin && user.Role != TimeTableRole.Fit)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var repository = new BookingRepository(context);
            var service = new StatisticsService();

            var amount = service.AmountOfMaintenanceBookings(repository, week);

            return Request.CreateResponse(HttpStatusCode.OK, amount);
        }

        [HttpGet]
        [SwaggerOperation("AmountOfBookings")]
        [Route("AmountOfBookings")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(int))]
        public HttpResponseMessage AmountOfBookings()
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var userRepository = new UserRepository(context);
            var userService = new UserService(userRepository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management && user.Role != TimeTableRole.Admin && user.Role != TimeTableRole.Fit)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var repository = new BookingRepository(context);
            var service = new StatisticsService();

            var amount = service.AmountOfBookings(repository);

            return Request.CreateResponse(HttpStatusCode.OK, amount);
        }
        #endregion

        #region AmountOfUsers
        [HttpGet]
        [SwaggerOperation("AmountOfUsers")]
        [Route("AmountOfUsers")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(int))]
        public HttpResponseMessage AmountOfUsers()
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var userRepository = new UserRepository(context);
            var userService = new UserService(userRepository);

            var user = userService.GetUserById(UserId);
            if (user.Role != TimeTableRole.Management && user.Role != TimeTableRole.Admin && user.Role != TimeTableRole.Fit)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Insufficient permissions.");

            var repository = new UserRepository(context);
            var service = new StatisticsService();

            var amount = service.AmountOfUsers(repository);

            return Request.CreateResponse(HttpStatusCode.OK, amount);
        }
        #endregion

    }
}
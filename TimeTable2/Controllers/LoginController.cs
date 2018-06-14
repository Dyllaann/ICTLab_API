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
    [RoutePrefix("api/Login")]
    public class LoginController : TimeTableApiController
    {
        [HttpPost]
        [SwaggerOperation("")]  
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        public HttpResponseMessage Login()
        {
            var context = new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
            var repository = new UserRepository(context);
            var service = new UserService(repository);

            var login = service.HandleUserLogin(UserProfile);
            if (login == null)
            {
                  return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                role = login.Role,
                rolestring = login.RoleString
            });
        }
    }
}
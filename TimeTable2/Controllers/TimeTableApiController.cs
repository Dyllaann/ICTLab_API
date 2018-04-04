using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Engine;
using TimeTable2.Models;
using TimeTable2.Services;

namespace TimeTable2.Controllers
{
    [RoutePrefix("api/main")]
    public class TimeTableApiController: ApiController
    {

        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            try
            {
                var header = controllerContext.Request.Headers.Authorization;
                if (header == null)
                {
                    return controllerContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "UnAuthorized");
                }
               
                //Debug section
                var debugSetting = WebConfigurationManager.AppSettings["debug"];
                var debug = bool.Parse(debugSetting);
                if (debug)
                {
                    if (header.Parameter == "tt2")
                    {
                        return await base.ExecuteAsync(controllerContext, cancellationToken);
                    }
                    return controllerContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "UnAuthorized");
                }

                //Actual Authentication section
                var authenticated = await APISerivce.Authorize(header.Parameter);
                if (authenticated == null)
                {
                    return controllerContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "UnAuthorized");
                }

                return await base.ExecuteAsync(controllerContext, cancellationToken);
            }
            catch (Exception e)
            {
                //var message = string.Format("error occured in {0} : {1}", controllerContext.Request.RequestUri, e.Message);
                return controllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, "InternalServerError");
            }
        }

        #region Test Methods 
        [HttpPost]
        [SwaggerOperation("testAuthentication")]
        [Route("testAuthentication")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GoogleUserProfile))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public async Task<HttpResponseMessage> Authorize(string token)
        {
            var isAuthorized = await APISerivce.Authorize(token);
            return (isAuthorized != null)
                ? Request.CreateResponse(HttpStatusCode.OK, isAuthorized)
                : Request.CreateResponse(HttpStatusCode.Unauthorized, "UnAuthorized");
        }

        [HttpGet]
        [SwaggerOperation("testRoom")]
        [Route("testRoom")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Classroom))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public HttpResponseMessage TestRoom()
        {
            var classroom = new Classroom
            {
                Capacity = 30,
                Id = Guid.NewGuid(),
                Maintenance = MaintenanceStatus.OK,
                RoomId = "H4.318"
            };

            return Request.CreateResponse(HttpStatusCode.OK, classroom);
        }
        #endregion
    }
}
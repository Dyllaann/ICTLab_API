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
using log4net;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Engine;
using TimeTable2.Models;
using TimeTable2.Services;

namespace TimeTable2.Controllers
{
    public class TimeTableApiController : ApiController
    {
        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            var logger = LogManager.GetLogger("TimeTableApiController");
            try
            {

                var header = controllerContext.Request.Headers.Authorization;
                if (header == null)
                {
                    logger.Info($"Empty Authorization header.");
                    return controllerContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "UnAuthorized");
                }
               
                //Debug section
                var debugSetting = WebConfigurationManager.AppSettings["debug"];
                var debug = bool.Parse(debugSetting);
                if (debug)
                {
                    if (header.Parameter == "tt2")
                    {
                        logger.Info($"Logged in with Debug Authentication.");
                        return await base.ExecuteAsync(controllerContext, cancellationToken);
                    }
                }

                //Actual Authentication section
                var authenticated = await APISerivce.Authorize(header.Parameter, logger);
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
    }
}
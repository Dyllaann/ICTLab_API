﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using log4net;
using TimeTable2.Services;

namespace TimeTable2.Controllers
{
    public class TimeTableApiController : ApiController
    {
        public string UserId { get; set; }

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
                    if (header.Scheme == "tt2")
                    {
                        logger.Info($"Logged in with Debug Authentication.");
                        return await base.ExecuteAsync(controllerContext, cancellationToken);
                    }
                }

                //Non-debug Authorization
                bool auth;
                switch (header.Scheme)
                {
                    case "Bearer":
                        auth = await GoogleAuth(header.Parameter, logger);
                        break;
                    case "Basic":
                        auth = BasicAuth(header.Parameter, logger);
                        break;
                    default:
                        return controllerContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "UnAuthorized");
                }

                if (auth)
                {
                    return await base.ExecuteAsync(controllerContext, cancellationToken);
                }
                return controllerContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "UnAuthorized");
            }
            catch (Exception e)
            {
                logger.Error($"An error occured", e);
                //var message = string.Format("error occured in {0} : {1}", controllerContext.Request.RequestUri, e.Message);
                return controllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, "InternalServerError");
            }
        }

        public bool BasicAuth(string token, ILog logger)
        {
            var data = Convert.FromBase64String(token);
            var decodedString = Encoding.UTF8.GetString(data);

            var dashboardUsername = WebConfigurationManager.AppSettings["BasicAuth.Dashboard.Username"];
            var dashboardToken = WebConfigurationManager.AppSettings["BasicAuth.Dashboard.Token"];
            var dashboard = $"{dashboardUsername}:{dashboardToken}";

            if (decodedString == dashboard)
            {
                UserId = "-1";
                return true;
            }

            var piRegex = Regex.Match(decodedString, "(Pi)(:)(.*)"); //Pi:H.1.110
            if (piRegex.Success)
            {
                UserId = piRegex.Groups[3].Value;
            }

            return false;
        }

        public async Task<bool> GoogleAuth(string token, ILog logger)
        {
            var authenticated = await APISerivce.Authorize(token, logger);
            if (authenticated == null) return false;

            UserId = authenticated.UserId;
            return true;
        }
    }
}
using System;
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
using TimeTable2.Engine;
using TimeTable2.Services;

namespace TimeTable2.Controllers
{
    public class TimeTableApiController : ApiController
    {
        public GoogleUserProfile UserProfile { get; set; }
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
                        UserId = "Kaj";
                        UserProfile = new GoogleUserProfile
                        {
                            UserId = UserId,
                            Email = "DEBUG",
                            Name = "DEBUG",
                            FamilyName = "DEBUG",
                            GivenName = "DEBUG"
                        };

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

            var dashboardRegex = Regex.Match(decodedString, "Dashboard:(.*):(.*):(.*):(.*):(.*)");
            if (dashboardRegex.Success)
            {
                var dashboardToken = WebConfigurationManager.AppSettings["BasicAuth.Dashboard.Token"];
                if (dashboardRegex.Groups[1].Value != dashboardToken) return false;
                if(!dashboardRegex.Groups[3].Value.EndsWith("@hr.nl")) return false;

                UserId = dashboardRegex.Groups[2].Value;
                UserProfile = new GoogleUserProfile
                {
                    UserId = UserId,
                    Email = dashboardRegex.Groups[3].Value,
                    Name = dashboardRegex.Groups[4].Value,
                    FamilyName = dashboardRegex.Groups[5].Value,
                    GivenName = dashboardRegex.Groups[4].Value + " " + dashboardRegex.Groups[5].Value
                };
                return true;
            }

            var piRegex = Regex.Match(decodedString, "(Pi)(:)(.*)"); //Pi:H.1.110
            if (piRegex.Success)
            {
                UserId = piRegex.Groups[3].Value;
                UserProfile = new GoogleUserProfile
                {
                    UserId = UserId,
                    Name = "Pi",
                    FamilyName = piRegex.Groups[3].Value,
                    GivenName = $"Pi {piRegex.Groups[3].Value}"
                };
                return true;
            }

            return false;
        }

        public async Task<bool> GoogleAuth(string token, ILog logger)
        {
            var authenticated = await APISerivce.Authorize(token, logger);
            if (authenticated == null) return false;

            UserProfile = authenticated;
            UserId = authenticated.UserId;
            return true;
        }
    }
}
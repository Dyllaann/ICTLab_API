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
using TimeTable2.Scraper;

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
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(data);
            }
            catch (JsonSerializationException)
            {
                return null;
            }

            if (tokenResponse.Aud == null) return null;
            tokenResponse.User.UserId = tokenResponse.Sub;
            return tokenResponse.Aud.Contains(ClientId)
                ? tokenResponse.User
                : null;
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

        [HttpGet]
        [SwaggerOperation("scrape")]
        [Route("scrape")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<string>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Scraping unable")]
        public HttpResponseMessage Scrape()
        {
            var scraper = new WebScraper();

            var listofrooms = new List<string>();
            listofrooms.Add("EXT");
            listofrooms.Add("H.1.110");
            listofrooms.Add("H.1.112");
            var html = scraper.Execute(listofrooms);
            return Request.CreateResponse(HttpStatusCode.OK, html);
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
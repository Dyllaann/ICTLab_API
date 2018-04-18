using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using TimeTable2.Engine;
using TimeTable2.Models;
using TimeTable2.Scraper;

namespace TimeTable2.Controllers
{
    [RoutePrefix("api/main")]
    public class TimeTableApiController: ApiController
    {
        const string Endpoint = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";
        private readonly string ClientId = WebConfigurationManager.AppSettings["Google.ClientId"];

        [HttpPost]
        [SwaggerOperation("testAuthentication")]
        [Route("testAuthentication")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserProfile))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Classroom schedule was not found")]
        public async Task<UserProfile> Authenticate(string token)
        {
            var url = string.Format(Endpoint, token);
            var client = new HttpClient();
            var response = client.GetAsync(url);
            var data = await response.Result.Content.ReadAsStringAsync();

            TokenResponse tokenResponse;
            try
            {
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

    public class TokenResponse
    {
        [JsonProperty("iss")]
        public string Iss { get; set; }
        [JsonProperty("sub")]
        public int Sub { get; set; }
        [JsonProperty("azp")]
        public string Azp { get; set; }
        [JsonProperty("aud")]
        public string Aud { get; set; }
        [JsonProperty("iat")]
        public int Iat { get; set; }
        [JsonProperty("exp")]
        public int Exp { get; set; }

        public UserProfile User { get; set; }
    }
}
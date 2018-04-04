using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json;
using TimeTable2.Engine;
using Google.Apis.Auth;

namespace TimeTable2.Services
{

    public static class APISerivce
    {
        private static readonly string ClientIdIos = WebConfigurationManager.AppSettings["GoogleClentId.ios"];
        private static readonly string ClientIdAndroid = WebConfigurationManager.AppSettings["GoogleClentId.android"];
        private static readonly string ClientIdWeb = WebConfigurationManager.AppSettings["GoogleClentId.web"];

        private const string Endpoint = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";

        public static async Task<GoogleUserProfile> Authorize(string token)
        {
            var client = new HttpClient();
            var url = string.Format(Endpoint, token);
            var response = client.GetAsync(url);
            var data = await response.Result.Content.ReadAsStringAsync();

            TokenResponse tokenResponse;
            try
            {
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(data);
            }
            catch (JsonSerializationException ex)
            {
                return null;
            }

            //EXP(iry) should not be passed
            var unixNow = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (tokenResponse.Exp > unixNow) return null;

            //AUD(ience) has to be valid ClientId of our application
            if (tokenResponse.Aud == null) return null;
            if (tokenResponse.Aud.Contains(ClientIdIos) || tokenResponse.Aud.Contains(ClientIdAndroid) || tokenResponse.Aud.Contains(ClientIdWeb))
            {
                //If everything is validated, create the userobject
                return new GoogleUserProfile(tokenResponse);
            }

            //If validation fails, return null
            return null;

        }
    }
}

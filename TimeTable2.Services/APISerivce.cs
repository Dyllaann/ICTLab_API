using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json;
using TimeTable2.Engine;
using Google.Apis.Auth;
using Google.Apis.Logging;
using log4net;
using TimeTable2.Tools;

namespace TimeTable2.Services
{

    public static class APISerivce
    {
        private static readonly string ClientIdIos = WebConfigurationManager.AppSettings["GoogleClientId.ios"];
        private static readonly string ClientIdAndroid = WebConfigurationManager.AppSettings["GoogleClientId.android"];
        private static readonly string ClientIdWeb = WebConfigurationManager.AppSettings["GoogleClientId.web"];

        private const string Endpoint = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";

        public static async Task<GoogleUserProfile> Authorize(string token, ILog logger)
        {
            var debugSetting = WebConfigurationManager.AppSettings["debug"];
            var debug = bool.Parse(debugSetting);
            if (debug)
            {
                logger.Info($"Token: {token}");
            }

            var client = new HttpClient();
            var url = string.Format(Endpoint, token);
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

            //EXP(iry) should not be passed
            var unixNow = TimeConversion.GetUnixNow();
            if (tokenResponse.Exp < unixNow)
            {
                logger.Info($"Token expired.");
                return null;
            }
            

            //AUD(ience) has to be valid ClientId of our application
            if (tokenResponse.Aud == null) return null;
            if (tokenResponse.Aud.Contains(ClientIdIos) || tokenResponse.Aud.Contains(ClientIdAndroid) || tokenResponse.Aud.Contains(ClientIdWeb))
            {
                var email = Regex.Match(tokenResponse.Email, "@(.*)$");
                if (email.Success && email.Groups[1].Value == "hr.nl")
                {
                    //If everything is okay: login
                    logger.Info($"Logged in!");
                    return new GoogleUserProfile(tokenResponse);
                }

                logger.Info("Not an @hr.nl email address");
                return null;
            }

            //If validation fails, return null
            logger.Info("Invalid audience.");
            return null;

        }
    }
}

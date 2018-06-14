using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTable2.Engine
{
    public class GoogleUserProfile
    {

        public GoogleUserProfile()
        {
        }

        public GoogleUserProfile(TokenResponse response)
        {
            Email = response.Email;
            EmailVerified = response.EmailVerified;
            Name = response.GivenName;
            FamilyName = response.FamilyName;
            GivenName = response.Name;
            Locale = response.Locale;
            Picture = response.Picture;
            UserId = response.Sub;
        }

        public string UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

    }
}

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

        public GoogleUserProfile(string userId, string email, bool emailVerified, string name, string picture, string givenName, string familyName, string locale)
        {
            UserId = userId;
            Email = email;
            EmailVerified = emailVerified;
            Name = name;
            Picture = picture;
            GivenName = givenName;
            FamilyName = familyName;
            Locale = locale;
        }

        public GoogleUserProfile(TokenResponse response)
        {
            Email = response.Email;
            EmailVerified = response.EmailVerified;
            Name = response.Name;
            FamilyName = response.FamilyName;
            GivenName = response.GivenName;
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

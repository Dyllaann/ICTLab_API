using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Google.Apis.Auth.OAuth2;

namespace TimeTable2.Controllers
{
    public sealed class JWTCertificateUrl
    {
        private JWTCertificateUrl(string value) { Value = value; }
        public string Value { get; set; }
        public static JWTCertificateUrl Google { get { return new JWTCertificateUrl("https://www.googleapis.com/oauth2/v1/certs"); } }
    }

    public sealed class JWTIssuer
    {
        private JWTIssuer(string value) { Value = value; }
        public string Value { get; set; }

        public static JWTIssuer None { get { return new JWTIssuer(""); } }
        public static JWTIssuer Google { get { return new JWTIssuer("accounts.google.com"); } }
    }

    public static class Utils
    {
        private const string beginCert = "-----BEGIN CERTIFICATE-----\\n";
        private const string endCert = "\\n-----END CERTIFICATE-----\\n";
        private static byte[][] getCertBytes(JWTCertificateUrl certificate)
        {
            // The request will be made to the authentication server.
            WebRequest request = WebRequest.Create(certificate.Value);

            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());

            string responseFromServer = reader.ReadToEnd();

            String[] split = responseFromServer.Split(':');

            // There are two certificates returned from Google
            byte[][] certBytes = new byte[2][];
            int index = 0;
            UTF8Encoding utf8 = new UTF8Encoding();
            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].IndexOf(beginCert) > 0)
                {
                    int startSub = split[i].IndexOf(beginCert);
                    int endSub = split[i].IndexOf(endCert) + endCert.Length;
                    certBytes[index] = utf8.GetBytes(split[i].Substring(startSub, endSub).Replace("\\n", "\n"));
                    index++;
                }
            }
            return certBytes;
        }

        public static bool CheckJWTToken(JWTCertificateUrl cert, JWTIssuer tokenIssuer, string appId, string idToken, ref Dictionary<string, string> data)
        {
            if (data == null)
                data = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(idToken))
                return false;

            //JwtSecurityToken token = new JwtSecurityToken(idToken);
            JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();

            Byte[][] certBytes = getCertBytes(cert);
            Dictionary<String, X509Certificate2> certificates = new Dictionary<String, X509Certificate2>();
            for (int i = 0; i < certBytes.Length; i++)
            {
                X509Certificate2 certificate = new X509Certificate2(certBytes[i]);
                certificates.Add(certificate.Thumbprint, certificate);
            }

            TokenValidationParameters tvp = new TokenValidationParameters()
            {
                ValidateActor = false, // check the profile ID

                ValidateAudience = true,
                ValidAudience = appId,

                ValidateIssuer = !string.IsNullOrEmpty(tokenIssuer.Value),
                ValidIssuer = tokenIssuer.Value,

                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                CertificateValidator = X509CertificateValidator.None,
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                    return identifier.Select(x =>
                    {
                        if (certificates.ContainsKey(x.Id.ToUpper()))
                        {
                            return new X509SecurityKey(certificates[x.Id.ToUpper()]);
                        }
                        return null;
                    }).First(x => x != null);
                },
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromSeconds(300) //5 minutes
            };

            bool res = false;
            try
            {
                // Validate using the provider
                SecurityToken validatedToken;
                ClaimsPrincipal cp = jsth.ValidateToken(idToken, tvp, out validatedToken);
                if (cp != null)
                {
                    foreach (var claim in cp.Claims)
                    {
                        var name = claim.Type;
                        //Delete the URL part just for convenient
                        if (name.StartsWith("http"))
                            name = name.Remove(0, name.LastIndexOf('/') + 1);
                        data.Add(name, claim.Value);
                    }
                    res = true;
                }

            }
            catch (Exception ex)
            {
            }

            return res;
        }
    }
}
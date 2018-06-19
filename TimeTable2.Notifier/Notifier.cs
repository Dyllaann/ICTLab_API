using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using Google.Cloud.Firestore;

namespace TimeTable2.Notifier
{
    public class Notifier : INotifier
    {
        public async Task Notify(string userId, string title, string message, string from)
        {
            var db = InitializeDb();

            var collection = db.Collection("notifications");
            var city = new
            {
                body = message,
                title,
                userId,
                Read = "False",
                date = DateTime.UtcNow,
                senderId = from
            };

            await collection.AddAsync(city);
        }

        private static FirestoreDb InitializeDb()
        {
            var projectId = WebConfigurationManager.AppSettings["FCM.Project.Id"];
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS")))
            {
                const string authPath = "keyfile.json";
                var path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var match = Regex.Match(path, "(TimeTable2.Notifier.DLL)+$");
                if (match.Success)
                {
                    var stringToRemove2 = match.Groups[0].Value;
                    path = path.Replace(stringToRemove2, "").Substring(8);
                    path += authPath;
                    // If it doesn't exist, create it.
                    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                }
            }

            var db = FirestoreDb.Create(projectId);
            return db;
        }
    }
}

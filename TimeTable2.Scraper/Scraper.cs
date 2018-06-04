using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json;
using TimeTable2.Engine;
using TimeTable2.Engine.Scraper;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Scraper
{
    public class WebScraper
    {
        public static string BaseUrl =
        "https://hint.hr.nl/xsp/public/InfApp/2018gr6/getLokaalRooster.xsp?sharedSecret={0}&element={1}&startDate={2}&endDate={3}&json=1";

        /// <summary>
        /// Scrape the list of 
        /// </summary>
        /// <param name="classRoomRepository"></param>
        /// <param name="filter"></param>
        /// <param name="quarter">The quarter of the year (1-4)</param>
        /// <param name="week">The week of the year (1-51)</param>
        /// <returns></returns>
        public async Task<List<Classroom>> Execute(IClassroomRepository classRoomRepository, int week)
        {
            
            var rooms = classRoomRepository.GetAllClassrooms().ToList();

            var startDate = FirstDateOfWeekIso8601(DateTime.Today.Year, week);
            var endDate = startDate.AddDays(4);

            var secret = WebConfigurationManager.AppSettings["Hint.Api.Secret"];

            foreach (var room in rooms)
            {
                var roomName = room.RoomId;

                var start = $"{startDate.Year}-{startDate.Month:00}-{startDate.Day:00}";
                var end = $"{endDate.Year}-{endDate.Month:00}-{endDate.Day:00}";

                var url = string.Format(BaseUrl, secret, roomName, start, end);

                var client = new HttpClient();
                var request = client.GetAsync(new Uri(url)).Result;
                if (!request.IsSuccessStatusCode) continue;

                var data = await request.Content.ReadAsStringAsync();
                var classroom = JsonConvert.DeserializeObject<ScraperClassroom>(data);

                var existingClassroom = classRoomRepository.GetClassroomWithCourses(classroom.ElementName);

                foreach (var course in room.Courses)
                {

                }
            }

           

            return rooms;
        }

        public static DateTime FirstDateOfWeekIso8601(int year, int weekOfYear)
        {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            var firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }
    }



}

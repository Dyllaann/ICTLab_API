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
using TimeTable2.Repository;
using TimeTable2.Repository.Interfaces;
using TimeTable2.Tools;

namespace TimeTable2.Scraper
{
    public class WebScraper
    {
        private IClassroomRepository ClassroomRepository { get;}
        private IBookingRepository BookingRepository { get; }
        private IClassRepository ClassRepository { get; }
        private IScraperRepository ScraperRepository { get; }

        public WebScraper(IClassroomRepository classroomRepository, IBookingRepository bookingRepository, IClassRepository classRepository, IScraperRepository scraperRepository)
        {
            ClassroomRepository = classroomRepository;
            BookingRepository = bookingRepository;
            ClassRepository = classRepository;
            ScraperRepository = scraperRepository;
        }


        public static string BaseUrl =
        "https://hint.hr.nl/xsp/public/InfApp/2018gr6/getLokaalRooster.xsp?sharedSecret={0}&element={1}&startDate={2}&endDate={3}&json=1";

        /// <summary>
        /// Scrape the list of 
        /// </summary>
        /// <param name="classRoomRepository"></param>
        /// <param name="bookingRepository"></param>
        /// <param name="week">The week of the year (1-51)</param>
        /// <returns></returns>
        public async Task<List<Classroom>> Execute(int week)
        {
            try
            {
                var rooms = ClassroomRepository.GetAllClassrooms().ToList();

                var startDate = TimeConversion.FirstDateOfWeekIso8601(DateTime.Today.Year, week);
                var endDate = startDate.AddDays(4);

                var secret = WebConfigurationManager.AppSettings["Hint.Api.Secret"];

                foreach (var room in rooms)
                {
                    //Format the request for the HINT API
                    var roomName = room.RoomId;
                    var start = $"{startDate.Year}-{startDate.Month:00}-{startDate.Day:00}";
                    var end = $"{endDate.Year}-{endDate.Month:00}-{endDate.Day:00}";

                    var url = string.Format(BaseUrl, secret, roomName, start, end);

                    //Prepare Client & Send Request
                    var client = new HttpClient();
                    var request = client.GetAsync(new Uri(url)).Result;
                    if (!request.IsSuccessStatusCode) continue;

                    //Read the response of the page
                    var data = await request.Content.ReadAsStringAsync();
                    ScraperClassroom classroom;
                    try
                    {
                        classroom = JsonConvert.DeserializeObject<ScraperClassroom>(data);
                    }
                    //Should only happen when the lesson is not an array but one lesson
                    catch (JsonSerializationException)
                    {
                        try
                        {
                            var tempClassroom = JsonConvert.DeserializeObject<ScraperClassroomSingleLesson>(data);
                            classroom = tempClassroom.ToScraperClassroom();
                        }
                        catch (JsonSerializationException ex)
                        {
                            continue;
                        }
                    }

                    //Find the classroom that is connected the one being scraped
                    var existingClassroom = ClassroomRepository.GetClassroomWithCourses(classroom.ElementName);

                    //For each lesson that is in the classroom from the API
                    foreach (var course in classroom.Lesson)
                    {
                        //Find an existing lesson or booking during these times
                        var availability = FindExisting(week, room, course);

                        switch (availability)
                        {
                            case Availability.Free:
                                var updatedClassroom = MapCourse(course, existingClassroom, week);
                                ClassroomRepository.AddOrUpdateClassroom(updatedClassroom);
                                break;
                            case Availability.Booked:
                                //TODO: Geboekt, maar nieuwe officiele les op dat moment
                                break;
                            case Availability.Duplicate:
                                //double scrape
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }

                return rooms;
            }
            catch (Exception e)
            {
                return null;
            }
        }



        private Classroom MapCourse(ScraperCourse course, Classroom classroom, int week)
        {
            if (course.Teacher == "KROOJ" && course.Subject == "INFLAB02")
            {
                //lol
            }
            try
            {
                var existingCourse = ScraperRepository.GetExistingCourseByRoomAndClassCode(course.Subject,
                    course.Text, week, course.DayNumber, course.PeriodStart, course.PeriodEnd);
                if (existingCourse != null)
                {
                    existingCourse.Rooms.Add(classroom);
                    ScraperRepository.AddOrUpdateCourse(existingCourse);
                    return classroom;
                }

                var lesson = new Course
                {
                    Description = course.Text,
                    CourseCode = course.Subject,
                    WeekDay = course.DayNumber,
                    Week = week,
                    Teacher = course.Teacher,
                    StartBlock = course.PeriodStart,
                    EndBlock = course.PeriodEnd,

                    Rooms = new List<Classroom>(),
                    Classes = new List<Class>()
                };

                if (course.Class != null)
                {
                    var classes = course.Class.Replace(" ", "").Split(',').ToList();
                    foreach (var @class in classes)
                    {
                        var existingClass = ScraperRepository.GetClassByName(@class);
                        lesson.Classes.Add(existingClass);
                    }
                }

                lesson.Rooms.Add(classroom);
                classroom.Courses.Add(lesson);
                return classroom;
            }
            catch (Exception e)
            {
                // ignored
                return null;
            }
        }

        private Availability FindExisting(int week, Classroom room, ScraperCourse course)
        {
            var found = room.Courses.FirstOrDefault(c => c.StartBlock == course.PeriodStart
                                                         && c.EndBlock == course.PeriodEnd
                                                         && c.WeekDay == course.DayNumber
                                                         && c.CourseCode == course.Subject);
            if (found != null)
            {
                return Availability.Duplicate;
            }

            var bookings = BookingRepository.GetBookingsByRoomAndWeek(room.RoomId, week);
            var existing = bookings.Where(l => l.WeekDay == course.DayNumber
                                               //Starts inside another booking
                                               && (course.PeriodStart >= l.StartBlock &&
                                                   course.PeriodStart <= l.EndBlock
                                                   //Ends inside another booking
                                                   || course.PeriodEnd >= l.StartBlock && course.PeriodEnd <= l.EndBlock
                                                   //Overlaps entire booking
                                                   || course.PeriodStart <= l.StartBlock &&
                                                   course.PeriodEnd >= l.EndBlock));

            return existing.Any()
                ? Availability.Booked
                : Availability.Free;
        }


        public enum Availability
        {
            Free = 0,
            Duplicate = 1,
            Booked = 2,
        }
    }



}

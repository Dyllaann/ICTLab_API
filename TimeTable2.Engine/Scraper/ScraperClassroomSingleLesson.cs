using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTable2.Engine.Scraper
{
    public class ScraperClassroomSingleLesson
    {
        public string ElementName { get; set; }
        public string ElementType { get; set; }
        public string SchoolId { get; set; }
        [JsonProperty("lesson")]
        public ScraperCourse Lesson { get; set; }


        public ScraperClassroom ToScraperClassroom()
        {
            var scraperclassroom = new ScraperClassroom
            {
                ElementName = ElementName,
                SchoolId = SchoolId,
                ElementType = ElementType,
                Lesson = new List<ScraperCourse>()
            };
            scraperclassroom.Lesson.Add(Lesson);
            return scraperclassroom;
        }
    }
}

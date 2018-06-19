using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTable2.Engine.Scraper
{
    public class ScraperClassroom
    {
        public string ElementName { get; set; }
        public string ElementType { get; set; }
        public string SchoolId { get; set; }
        [JsonProperty("lesson")]
        public List<ScraperCourse> Lesson { get; set; }
    }
}

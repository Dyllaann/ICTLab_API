using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTable2.Engine.Scraper
{
    public class ScraperCourse
    {
        /// <summary>
        /// Unique Lesson ID
        /// <example>13860</example>  
        /// </summary>
        [JsonProperty("LSID")]
        public string Lsid { get; set; }

        /// <summary>
        /// The DATE the lesson is given. Without time
        /// <example>2018-05-31T00:00:00+02:00</example>
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Number of the week the lesson is given in
        /// <example>3</example>
        /// </summary>
        public int DayNumber { get; set; }

        /// <summary>
        /// The identifier of the block the lesson starts at
        /// <example>4</example>
        /// </summary>
        public int PeriodStart { get; set; }

        /// <summary>
        /// The identifier of the block the lesson ends at
        /// <example>7</example>
        /// </summary>
        public int PeriodEnd { get; set; }

        /// <summary>
        /// The time the lesson starts at
        /// <example>"1120"</example>
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// The time the lesson ends at
        /// <example>"1550"</example>
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// The course description
        /// <example>ICT-Lab 02</example>
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Unknown
        /// </summary>
        public string StatFlags { get; set; }

        /// <summary>
        /// The class(ses) the course is given for
        /// <example>INF3A, INF3B, INF3C</example>
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// The course code
        /// <example>INFANL019</example>
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The teacher that gives this course
        /// <example>OMARA</example>
        /// </summary>
        public string Teacher { get; set; }

        /// <summary>
        /// List of rooms the course is given at
        /// </summary>
        [JsonProperty("room")]
        public Room Room { get; set; }
    }


    public class Room
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("building")]
        public string Building { get; set; }
        [JsonProperty("latitude")]
        public string Latitude { get; set; }
        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}
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
        [JsonProperty("LSID")]
        public string Lsid { get; set; }
        public string Date { get; set; }
        public int DayNumber { get; set; }
        public int PeriodStart { get; set; }
        public int PeriodEnd { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Text { get; set; }
        public string StatFlags { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public string Teacher { get; set; }


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
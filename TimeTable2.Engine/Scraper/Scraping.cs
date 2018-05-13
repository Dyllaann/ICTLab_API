using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable2.Engine.Scraper
{
    public class Scraping
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        private List<Classroom> Classrooms { get; set; }
    }
}

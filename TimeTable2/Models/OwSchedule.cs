using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTable2.Models
{
    public class OwSchedule
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public IList<OwCourse> Courses { get; set; }
    }
}
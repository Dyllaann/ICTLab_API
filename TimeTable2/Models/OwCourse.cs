using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTable2.Models
{
    public class OwCourse
    {
        public string RoomCode { get; set; }
        public string TeacherCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
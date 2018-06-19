using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTable2.Engine.Bookings
{
    public class FilterClassroom
    {
        public string RoomId { get; set; }
        public int Capacity { get; set; }
        public MaintenanceStatus Maintenance { get; set; }
        public int FreeUntill { get; set; }


        [JsonIgnore]
        public List<Course> Courses { get; set; }

        public FilterClassroom(Classroom classroom)
        {
            RoomId = classroom.RoomId;
            Capacity = classroom.Capacity;
            Maintenance = classroom.Maintenance;
            Courses = classroom.Courses;
        }
    }
}

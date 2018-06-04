using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Tests.Resources
{
    public class TestClassroomRepository : IClassroomRepository
    {
        public List<Course> Courses { get; set; }
        public List<Classroom> Classrooms { get; set; }

        public ICollection<Classroom> GetAllClassrooms()
        {
            return Classrooms;
        }



        public ICollection<Course> GetCoursesByRoomAndWeek(string roomCode, int week)
        {
            var courses = Courses.Where(c => c.Week == week && c.Classroom.RoomId == roomCode).ToList();
            return courses;
        }

        public ICollection<Course> GetCoursesByClassAndWeek(string classCode, int week)
        {
            var courses = Courses.Where(c => c.Week == week && c.Class == classCode).ToList();
            return courses;
        }
    }
}

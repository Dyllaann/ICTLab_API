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
            var courses = Courses.Where(c => c.Week == week && c.Rooms.Contains(c.Rooms.FirstOrDefault(r => r.RoomId == roomCode))).ToList();
            return courses;
        }

        public ICollection<Course> GetCoursesByClassAndWeek(string classCode, int week)
        {
            var courses = Courses.Where(c => c.Week == week && c.Classes.Find(d => d.Name == classCode) != null).ToList();
            return courses;
        }

        public Classroom GetClassroomWithCourses(string roomCode)
        {
            return Classrooms.FirstOrDefault(c => c.RoomId == roomCode);
        }

        public Classroom AddOrUpdateClassroom(Classroom classroom)
        {
            Classrooms.Remove(classroom);
            Classrooms.Add(classroom);
            return classroom;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Repository
{
    public class ClassroomRepository : DbRepository, IClassroomRepository
    {
        public ClassroomRepository(DbContext context) : base(context)
        {
        }


        public ICollection<Classroom> GetAllClassrooms()
        {
            return Context.Set<Classroom>().ToList();
        }



        public ICollection<Course> GetCoursesByRoomAndWeek(string roomCode, int week)
        {
            var courses = Context.Set<Course>().Include(c => c.Classes).Include(c => c.Rooms)
                .Where(c => c.Week == week && c.Rooms.Select(r => r.RoomId).Contains(roomCode)).ToList();
            return courses;
        }

        public ICollection<Course> GetCoursesByClassAndWeek(string classCode, int week)
        {
            var courses = Context.Set<Course>().Include(c => c.Classes).Include(c => c.Rooms)
                .Where(c => c.Week == week && c.Classes.Select(d => d.Name).Contains(classCode)).ToList();
            return courses;
        }

        public Classroom GetClassroomWithCourses(string roomCode)
        {
            return Context.Set<Classroom>().Include(c => c.Courses).FirstOrDefault(c => c.RoomId == roomCode);
        }

        public Classroom GetClassroomById(string roomCode)
        {
            return Context.Set<Classroom>().FirstOrDefault(c => c.RoomId == roomCode);
        }

        public Classroom AddOrUpdateClassroom(Classroom classroom)
        {
            Context.Set<Classroom>().AddOrUpdate(classroom);
            Context.SaveChanges();
            return classroom;
        }

        public List<Classroom> GetAllClassroomsWithCourses(int week)
        {
            var classrooms = GetAllClassrooms();
            foreach (var classroom in classrooms)
            {
                var courses = Context.Set<Course>().Where(c => c.Rooms.Select(r => r.RoomId).Contains(classroom.RoomId) && c.Week == week).ToList();
                classroom.Courses = courses;
            }

            return classrooms.ToList();
        }

        public List<Classroom> GetAllClassroomsWithCoursesRemoveEmpty(int week)
        {
            var classrooms = GetAllClassrooms();
            var classroomsWithClasses = new List<Classroom>();
            foreach (var classroom in classrooms)
            {
                var courses = Context.Set<Course>().Where(c => c.Rooms.Select(r => r.RoomId).Contains(classroom.RoomId) && c.Week == week).ToList();
                if (courses.Count == 0) continue;

                classroom.Courses = courses;
                classroomsWithClasses.Add(classroom);
            }

            return classroomsWithClasses;
        }


        public List<Classroom> GetAllClassroomsWithBookingsRemoveEmpty(int week)
        {
            var classrooms = Context.Set<Classroom>().ToList();
            var classroomsWithBookings = new List<Classroom>();
            var allBookings = Context.Set<Booking>().ToList();
            foreach (var classroom in classrooms)
            {
                var bookings = allBookings.Where(b => b.Week == week && b.Classroom == classroom.RoomId).ToList();
                if (bookings.Count == 0)
                {
                }
                else
                {
                    classroom.Bookings = bookings;
                    classroomsWithBookings.Add(classroom);
                }
            }

            return classroomsWithBookings;
        }

        public List<int> GetAvailableWeeks()
        {
            return Context.Set<Course>().Select(c => c.Week).Distinct().ToList();
        }
    }
}

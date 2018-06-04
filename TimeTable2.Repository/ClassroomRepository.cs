﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var courses = Context.Set<Course>().Where(c => c.Week == week && c.Classroom.RoomId == roomCode).ToList();
            return courses;
        }

        public ICollection<Course> GetCoursesByClassAndWeek(string classCode, int week)
        {
            var courses = Context.Set<Course>().Where(c => c.Week == week && c.Class == classCode).ToList();
            return courses;
        }

        public Classroom GetClassroomWithCourses(string roomCode)
        {
            return Context.Set<Classroom>().Include(c => c.Courses).FirstOrDefault(c => c.RoomId == roomCode);
        }
    }
}

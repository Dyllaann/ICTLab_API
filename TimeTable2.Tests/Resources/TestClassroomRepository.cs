﻿using System;
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

        public TestClassroomRepository()
        {
            Courses = new List<Course>();
            Classrooms = new List<Classroom>();
        }

        public ICollection<Classroom> GetAllClassrooms()
        {
            return Classrooms;
        }



        public ICollection<Course> GetCoursesByRoomAndWeek(string roomCode, int week)
        {
            var courses = Courses.Where(c => c.Week == week && c.Rooms != null && c.Rooms.Contains(c.Rooms.FirstOrDefault(r => r.RoomId == roomCode))).ToList();
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

        public Classroom GetClassroomById(string roomCode)
        {
            return Classrooms.FirstOrDefault(c => c.RoomId == roomCode);
        }

        public Classroom AddOrUpdateClassroom(Classroom classroom)
        {
            Classrooms.Remove(classroom);
            Classrooms.Add(classroom);
            return classroom;
        }

        public List<Classroom> GetAllClassroomsWithCourses(int week)
        {
            return Classrooms;
        }

        public List<Classroom> GetAllClassroomsWithCoursesRemoveEmpty(int week)
        {
            var classrooms = GetAllClassrooms();
            return classrooms.Where(classroom => classroom.Courses.Count != 0).ToList();
        }

        public List<Classroom> GetAllClassroomsWithBookingsRemoveEmpty(int week)
        {
            var classrooms = GetAllClassrooms();
            return classrooms.Where(classroom => classroom.Bookings.Count != 0).ToList();
        }

        public List<Classroom> GetAllClassroomsWithCoursesAndBookings(int week)
        {
            return Classrooms;
        }

        public List<int> GetAvailableWeeks()
        {
            return Courses.OrderByDescending(course => course.Week).Select(c => c.Week).Distinct().ToList();
        }
    }
}

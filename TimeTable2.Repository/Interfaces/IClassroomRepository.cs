using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;

namespace TimeTable2.Repository.Interfaces
{
    public interface IClassroomRepository
    {
        ICollection<Classroom> GetAllClassrooms();


        ICollection<Course> GetCoursesByRoomAndWeek(string roomCode, int week);
        ICollection<Course> GetCoursesByClassAndWeek(string classCode, int week);
        Classroom GetClassroomWithCourses(string roomCode);
        Classroom GetClassroomById(string roomCode);


        Classroom AddOrUpdateClassroom(Classroom classroom);
        List<Classroom> GetAllClassroomsWithCourses(int week);  
        List<Classroom> GetAllClassroomsWithCoursesRemoveEmpty(int week);
        List<Classroom> GetAllClassroomsWithBookingsRemoveEmpty(int week);
        List<int> GetAvailableWeeks();
    }
}

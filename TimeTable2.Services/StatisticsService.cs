using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Engine.Statistics;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Services
{
    public class StatisticsService
    {
        public List<MostUsedRoom> GetMostUsedClassroomsByLessons(IClassroomRepository classroomRepository, int amount, int week)
        {
            var allRooms = classroomRepository.GetAllClassroomsWithCoursesRemoveEmpty(week);
            var orderedRooms = allRooms.OrderByDescending(r => r.Courses.Count).Take(amount).ToList();
            var statistics = orderedRooms.Select(s => new MostUsedRoom
            {
                RoomId = s.RoomId,
                Top = orderedRooms.IndexOf(s),
                AmountOfLessons = s.Courses.Count,
            }).ToList();
            return statistics;
        }

        public List<MostUsedRoom> GetMostUsedClassroomsByBookings(IClassroomRepository classroomRepository, int amount, int week)
        {
            var allRooms = classroomRepository.GetAllClassroomsWithBookingsRemoveEmpty(week);
            var orderedRooms = allRooms.OrderByDescending(r => r.Bookings.Count).Take(amount).ToList();
            var statistics = orderedRooms.Select(s => new MostUsedRoom
            {
                RoomId = s.RoomId,
                Top = orderedRooms.IndexOf(s),
                AmountOfLessons = s.Bookings.Count,
            }).ToList();
            return statistics;
        }
    }

}

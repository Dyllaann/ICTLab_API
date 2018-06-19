using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using TimeTable2.Engine;
using TimeTable2.Repository;
using TimeTable2.Repository.Interfaces;
using TimeTable2.Tools;

namespace TimeTable2.Services
{
    public class TimeTableService
    {
        private IClassroomRepository ClassroomRepository { get; }

        #region CTOR
        public TimeTableService(IClassroomRepository classroomRepository)
        {
            ClassroomRepository = classroomRepository;
        }
        #endregion

        #region Public Methods
        public ICollection<Course> GetClassroomScheduleByCodeAndWeek(string roomCode, int week)
        {
            return ClassroomRepository.GetCoursesByRoomAndWeek(roomCode, week);
        }

        public ICollection<Course> GetClassScheduleByCodeAndWeek(string classCode, int week)
        {
            return ClassroomRepository.GetCoursesByClassAndWeek(classCode, week);
        }

        public List<Classroom> FindEmpty(int start, int end, int dayofweek)
        {
            var weekNow = DateTime.Now.DayOfYear / 7 + 1;
            var allClassrooms = ClassroomRepository.GetAllClassroomsWithCourses(weekNow);
            var emptyRooms = new List<Classroom>();

            foreach (var room in allClassrooms)
            {
                var block = CourseAvailability(room.Courses, weekNow, start, end, dayofweek);
                if(block.Count > 0)
                {
                    continue;
                }

                room.Courses = null;
                emptyRooms.Add(room);
            }

            return emptyRooms;
        }

        public List<int> GetAvailableWeeks()
        {
            return ClassroomRepository.GetAvailableWeeks();
        }
        #endregion

        #region Private Methods
        private List<Course> CourseAvailability(ICollection<Course> existingLessons, int week, int start, int end, int dayofweek)
        {
            var blocking = existingLessons.Where(l => l.Week == week
                                                      && l.WeekDay == dayofweek
                                                      //Starts inside another lesson
                                                      && ( start >= l.StartBlock && start <= end
                                                      //Ends inside another lesson
                                                      || end >= l.StartBlock && end <= l.EndBlock
                                                      //Overlaps entire lesson
                                                      || start <= l.StartBlock && end >= l.EndBlock))
                .ToList();


            return blocking;
        }
        #endregion
    }
}

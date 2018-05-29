using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository;
using TimeTable2.Repository.Interfaces;

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


        public ICollection<Course> GetClassroomScheduleByCodeAndWeek(string roomCode, int week)
        {
            return ClassroomRepository.GetCoursesByRoomAndWeek(roomCode, week);
        }

        public ICollection<Course> GetClassScheduleByCodeAndWeek(string classCode, int week)
        {
            return ClassroomRepository.GetCoursesByClassAndWeek(classCode, week);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository;

namespace TimeTable2.Services
{
    public class TimeTableService
    {
        private IRepository ClassroomRepository { get; }

        public TimeTableService(IRepository classroomRepository)
        {
            ClassroomRepository = classroomRepository;
        }

        public Classroom GetClassroomById(string roomId)
        {
            return ClassroomRepository.GetClassroomById(roomId);
        }
    }
}

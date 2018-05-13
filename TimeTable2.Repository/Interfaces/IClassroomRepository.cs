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
        Classroom GetClassroomById(string roomId);
        ICollection<Course> GetCoursesByRoomAndWeek(string roomCode, int week);
        ICollection<Course> GetCoursesByClassAndWeek(string classCode, int week);
    }
}

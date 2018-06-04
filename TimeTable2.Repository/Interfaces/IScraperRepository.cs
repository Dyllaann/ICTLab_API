using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;

namespace TimeTable2.Repository.Interfaces
{
    public interface IScraperRepository
    {
        void AddOrUpdateClassrooms(List<Classroom> classrooms);
        Class GetClassByName(string className);

        Course GetExistingCourseByRoomAndClassCode(string courseCode, string description, int week, int weekday,
            int startBlock, int endBlock);

        Course AddOrUpdateCourse(Course course);
    }
}

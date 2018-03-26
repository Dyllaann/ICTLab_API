using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;

namespace TimeTable2.Repository
{
    public interface IRepository
    {
        Classroom GetClassroomById(string roomId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Services
{
    public class ClassroomService
    {
        private IClassroomRepository ClassroomRepository { get; }

        public ClassroomService(IClassroomRepository classroomRepository)
        {
            ClassroomRepository = classroomRepository;
        }


        public List<Classroom> GetAllClassrooms()
        {
            return ClassroomRepository.GetAllClassrooms().ToList();
        }
    }
}

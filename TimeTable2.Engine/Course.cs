using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable2.Engine
{
    public class Course
    {
        public Guid Id { get; set; }
        public int startBlok { get; set; }
        public int EndBlok { get; set; }
        public Classroom Room { get; set; }
        public string Docent { get; set; }
        public string VakCode { get; set; }
    }
}

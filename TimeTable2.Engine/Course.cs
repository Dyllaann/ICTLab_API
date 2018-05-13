using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTable2.Engine
{
    public class Course
    {
        public Guid Id { get; set; }
        public int Week { get; set; }
        public int WeekDay { get; set; }
        public int startBlok { get; set; }
        public int EndBlok { get; set; }
        public string Docent { get; set; }
        public string VakCode { get; set; }
        public string Klas { get; set; }

        [JsonIgnore]
        public Classroom Classroom { get; set; }
    }
}

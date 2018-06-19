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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public int Week { get; set; }
        public int WeekDay { get; set; }
        public int StartBlock { get; set; }
        public int EndBlock { get; set; }
        public string Teacher { get; set; }
        public string Description { get; set; }
        public string CourseCode { get; set; }

        public List<Class> Classes { get; set; }
        public List<Classroom> Rooms { get;set; }
    }
}

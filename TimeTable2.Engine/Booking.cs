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
    public class Booking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Key { get; set; }
        public int Week { get; set; }
        public int WeekDay { get; set; }
        public int StartBlock { get; set; }
        public int EndBlock { get; set; }
        public string Guests { get; set; }
        public string Classroom { get; set; }


        [JsonIgnore]
        public string Owner { get; set; }
        [JsonIgnore]
        public Classroom Lokaal { get; set; }
    }
}

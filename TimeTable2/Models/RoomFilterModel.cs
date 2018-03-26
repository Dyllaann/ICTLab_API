using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTable2.Models
{
    public class RoomFilterModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Capacity { get; set; }
    }
}
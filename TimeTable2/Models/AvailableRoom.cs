using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeTable2.Engine;

namespace TimeTable2.Models
{
    public class AvailableRoom
    {
        public string RoomCode { get; set; }
        
        public DateTime AvailableFrom { get; set; }

        public DateTime AvailableTo { get; set; }
        public int Capacity { get; set; }
    }
}
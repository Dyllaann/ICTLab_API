using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTable2.Models
{
    public class BookRoomModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string RoomCode { get; set; }
    }
}
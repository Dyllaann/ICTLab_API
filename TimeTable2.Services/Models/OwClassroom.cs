using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;

namespace TimeTable2.Services.Models
{
    public class OwClassroom
    {
        public string RoomId { get; set; }

        public int Capacity { get; set; }

        public MaintenanceStatus Maintenance { get; set; }

        public AvailabilityStatus Status { get; set; }
    }
}

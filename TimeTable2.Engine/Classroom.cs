using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTable2.Engine
{
    public class Classroom
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string RoomId { get; set; }

        public int Capacity { get; set; }

        public MaintenanceStatus Maintenance { get; set; }
    }
}
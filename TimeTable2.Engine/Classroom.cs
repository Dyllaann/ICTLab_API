using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTable2.Engine
{
    public class Classroom
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public Guid Id { get; set; }

        [Index("RoomId_Index", IsUnique = true)]
        [MaxLength(25)]
        public string RoomId { get; set; }

        public int Capacity { get; set; }

        public MaintenanceStatus Maintenance { get; set; }

        
        public IList<Course> Courses { get; set; }
    }
}
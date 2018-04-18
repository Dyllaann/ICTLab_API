using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;

namespace TimeTable2.Data
{
    public class TimeTableContext : DbContext
    {
        public DbSet<User> Sessions { get; set; }

        public TimeTableContext(string dbString) : base(dbString)
        {
            
        }
    }
}

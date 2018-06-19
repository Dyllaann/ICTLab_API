using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MySql.Data.Entity;
using TimeTable2.Engine;
using TimeTable2.Engine.Bookings;

namespace TimeTable2.Data
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class TimeTableContext : DbContext
    {
        public TimeTableContext(string dbString) : base(dbString)
        {
            /*
            var logger = LogManager.GetLogger("DbContext");
            Database.Log = x => logger.Info(x);
            */
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Class> Classes { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("");
        }
    }
}

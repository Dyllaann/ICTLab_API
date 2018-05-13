using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Entity;
using TimeTable2.Engine;

namespace TimeTable2.Data
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class TimeTableContext : DbContext
    {
        public TimeTableContext(string dbString) : base(dbString)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Course> Courses { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("");
        }
    }
}

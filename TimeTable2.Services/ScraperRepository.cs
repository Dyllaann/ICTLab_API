using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Services
{
    public class ScraperRepository : DbRepository, IScraperRepository
    {
        public ScraperRepository(DbContext context) : base(context){}


        public List<Classroom> GetAllClassrooms()
        {
            return Context.Set<Classroom>().Include(c => c.Courses).ToList();
        }

        public void AddOrUpdateClassrooms(List<Classroom> classrooms)
        {
            Context.Set<Classroom>().AddOrUpdate(classrooms.ToArray());
            Context.SaveChanges();
        }
    }
}

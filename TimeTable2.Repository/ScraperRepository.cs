using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Repository
{
    public class ScraperRepository : DbRepository, IScraperRepository
    {
        public ScraperRepository(DbContext context) : base(context) { }


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

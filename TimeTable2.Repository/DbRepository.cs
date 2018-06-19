using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable2.Repository
{
    public class DbRepository
    {
        public readonly DbContext Context;

        public DbRepository(DbContext context)
        {
            Context = context;
        }
    }
}

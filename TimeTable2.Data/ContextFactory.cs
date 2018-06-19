using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace TimeTable2.Data
{
    public class ContextFactory : IDbContextFactory<TimeTableContext>
    {
        public TimeTableContext Create()
        {
            return new TimeTableContext(WebConfigurationManager.AppSettings["DbConnectionString"]);
        }
    }
}

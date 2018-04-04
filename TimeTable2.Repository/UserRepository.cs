using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext context;

        public UserRepository(DbContext context)
        {
            this.context = context;
        }

        public void CreateUser(User user)
        {
            context.Set<User>().Add(user);
            context.SaveChanges();
        }
    }
}

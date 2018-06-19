using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Repository
{
    public class UserRepository : DbRepository, IUserRepository
    {
        public UserRepository(DbContext context) : base(context) { }

        public void CreateUser(User user)
        {
            Context.Set<User>().Add(user);
            Context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            Context.Set<User>().AddOrUpdate(user);
            Context.SaveChanges();
        }

        public User GetUserById(string userId)
        {
            var user = Context.Set<User>().FirstOrDefault(u => u.UserId == userId);
            return user;
        }

        public List<User> GetAllUsers()
        {
            var users = Context.Set<User>().ToList();
            return users;
        }
    }
}

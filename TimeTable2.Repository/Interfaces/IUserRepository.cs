using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;

namespace TimeTable2.Repository.Interfaces
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        void UpdateUser(User user);
        User GetUserById(string userUserId);
        List<User> GetAllUsers();
    }
}

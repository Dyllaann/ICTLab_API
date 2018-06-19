using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Services
{
    public class UserService
    {
        public IUserRepository UserRepository { get; set; }

        public UserService(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public User HandleUserLogin(GoogleUserProfile user)
        {
            if (user == null) return null;

            var existingUser = UserRepository.GetUserById(user.UserId);
            if (existingUser == null)
            {
                var newUser = new User
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FamilyName = user.FamilyName,
                    GivenName = user.GivenName,
                    Name = user.Name,
                    CreatedAt = DateTime.UtcNow,
                    Role = TimeTableRole.Student
                };
                UserRepository.CreateUser(newUser);
                return newUser;
            }
            
            existingUser.LastLogin = DateTime.UtcNow;
            UserRepository.UpdateUser(existingUser);
            return existingUser;
        }

        public User GetUserById(string userId)
        {
            return UserRepository.GetUserById(userId);
        }

    }
}

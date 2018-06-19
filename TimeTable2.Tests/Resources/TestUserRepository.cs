using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Tests.Resources
{
    public class TestUserRepository : IUserRepository
    {
        public List<User> Users { get; set; }

        public TestUserRepository()
        {
            Users = new List<User>();
        }



        public void CreateUser(User user)
        {
            Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            Users.Remove(user);
            Users.Add(user);
        }

        public User GetUserById(string userId)
        {
            var user = Users.FirstOrDefault(u => u.UserId == userId);
            return user;
        }

        public List<User> GetAllUsers()
        {
            var users = Users.ToList();
            return users;
        }

        #region Generation of objects
        public static User GenerateUser()
        {
            return new User
            {
                UserId = "TESTMETHOD",
                Name = "Test",
                FamilyName = "Method",
                GivenName = "Test Method",
                Email = "testuser@hr.nl",
                Role = TimeTableRole.Student,
                RoleString = "Student",
                CreatedAt = new DateTime(2018, 6, 1, 14, 36, 07),
                LastLogin = new DateTime(2018, 6, 5, 18, 54, 18)
            };
        }

        public static User GenerateUser(string userId)
        {
            return new User
            {
                UserId = userId,
                Name = "Test",
                FamilyName = "Method",
                GivenName = "Test Method",
                Email = "testuser@hr.nl",
                Role = TimeTableRole.Student,
                RoleString = "Student",
                CreatedAt = new DateTime(2018, 6, 1, 14, 36, 07),
                LastLogin = new DateTime(2018, 6, 5, 18, 54, 18)
            };
        }

        public static User GenerateUser(string userId, TimeTableRole role)
        {
            return new User
            {
                UserId = userId,
                Name = "Test",
                FamilyName = "Method",
                GivenName = "Test Method",
                Email = "testuser@hr.nl",
                Role = role,
                RoleString = role.ToString(),
                CreatedAt = new DateTime(2018, 6, 1, 14, 36, 07),
                LastLogin = new DateTime(2018, 6, 5, 18, 54, 18)
            };
        }

        public static GoogleUserProfile GenerateGoogleUser()
        {
            return new GoogleUserProfile()
            {
                UserId = "TESTMETHOD",
                Name = "Test",
                FamilyName = "Method",
                GivenName = "Test Method",
                Email = "testuser@hr.nl",
            };
        }
        #endregion
    }
}

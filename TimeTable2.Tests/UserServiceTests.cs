using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTable2.Engine;
using TimeTable2.Services;
using TimeTable2.Tests.Resources;

namespace TimeTable2.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void Login()
        {
            //Init
            var repo = new TestUserRepository();
            var service = new UserService(repo);
            
            //Given
            var googleUser = TestUserRepository.GenerateGoogleUser();
            var user = TestUserRepository.GenerateUser();
            repo.Users.Add(user);

            //When
            var timeNow = user.LastLogin;
            var updatedUser = service.HandleUserLogin(googleUser);
            var updatedTime = updatedUser.LastLogin;

            //Then
            Assert.AreNotEqual(timeNow, updatedTime, "Last login value did not change.");
        }

        [TestMethod]
        public void LoginNewUser()
        {
            //Init
            var repo = new TestUserRepository();
            var service = new UserService(repo);

            //Given
            var googleUser = TestUserRepository.GenerateGoogleUser();

            //When
            var now = DateTime.UtcNow;
            var newUser = service.HandleUserLogin(googleUser);

            //Then
            Assert.IsTrue(now <= newUser.CreatedAt, "CreatedAt is not set when creating");
            Assert.AreEqual(googleUser.UserId, newUser.UserId, "UserID's don't match.");
            Assert.AreEqual(newUser.Role, TimeTableRole.Student, "Default role should be student");
        }


    }
}

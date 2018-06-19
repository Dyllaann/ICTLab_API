using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTable2.Engine;
using TimeTable2.Engine.Bookings;
using TimeTable2.Engine.Management;
using TimeTable2.Services;
using TimeTable2.Tests.Resources;

namespace TimeTable2.Tests
{
    [TestClass]
    public class ManagementServiceTests
    {
        [TestMethod]
        public void UserEdit()
        {
            //Init
            var repository = new TestUserRepository();
            var service = new ManagementService();

            //Given
            var user = TestUserRepository.GenerateUser();
            repository.Users.Add(user);
            var edit = new UserEdit
            {
                UserId = "TESTMETHOD",
                NewRole = TimeTableRole.Management
            };

            //When
            var updatedUser = service.EditUser(repository, edit);

            //Then
            var newuser = repository.Users[0];
            Assert.AreEqual(TimeTableRole.Management, newuser.Role, "Role was not updated");
            Assert.AreEqual("Management", newuser.RoleString, "RoleString was not updated");
        }

        [TestMethod]
        public async Task MaintenanceBooking()
        {
            //Init
            var classroomRepo = new TestClassroomRepository();
            var userRepo = new TestUserRepository();
            var bookingRepo = new TestBookingRepository();

            var managementService = new ManagementService();
            var bookingService = new BookingService(bookingRepo, classroomRepo);

            var notifier = new TestNotifier();

            //Given
            var user = TestUserRepository.GenerateUser();
            var booking = new Booking
            {
                Classroom = "H.1.110",
                Week = 25,
                WeekDay = 2,
                StartBlock = 1,
                EndBlock = 10,
                Type = BookingType.Maintenance,
                Owner = user.UserId,
            };

            //When
            var actualBookingResult = await managementService.MaintenanceBooking(classroomRepo, bookingRepo, userRepo,
                bookingService, booking, user.UserId, notifier);

            //Then
            Assert.AreEqual(BookingAvailability.Success, actualBookingResult);

        }

        [TestMethod]
        public async Task MaintenanceBookingWithBlocking()
        {
            //Init
            var classroomRepo = new TestClassroomRepository();
            var userRepo = new TestUserRepository();
            var bookingRepo = new TestBookingRepository();

            var managementService = new ManagementService();
            var bookingService = new BookingService(bookingRepo, classroomRepo);

            var notifier = new TestNotifier();

            //Given
            var user = TestUserRepository.GenerateUser();
            var user2 = TestUserRepository.GenerateUser("TESTUSER2");

            userRepo.Users.Add(user);
            userRepo.Users.Add(user2);

            var bookingBlocking = new Booking
            {
                Classroom = "H.1.110",
                Week = 25,
                WeekDay = 2,
                StartBlock = 2,
                EndBlock = 4,
                Type = 0,
                Owner = user2.UserId,
            };
            bookingRepo.Bookings.Add(bookingBlocking);


            //When
            var booking = new Booking
            {
                Classroom = "H.1.110",
                Week = 25,
                WeekDay = 2,
                StartBlock = 1,
                EndBlock = 10,
                Type = BookingType.Maintenance,
                Owner = user.UserId,
            };
            var actualBookingResult = await managementService.MaintenanceBooking(classroomRepo, bookingRepo, userRepo,
                bookingService, booking, user.UserId, notifier);

            //Then
            Assert.AreEqual(BookingAvailability.Success, actualBookingResult);

        }

        [TestMethod]
        public async Task MaintenanceBookingWithRealBlocking()
        {
            //Init
            var classroomRepo = new TestClassroomRepository();
            var userRepo = new TestUserRepository();
            var bookingRepo = new TestBookingRepository();

            var managementService = new ManagementService();
            var bookingService = new BookingService(bookingRepo, classroomRepo);

            var notifier = new TestNotifier();

            //Given
            var user = TestUserRepository.GenerateUser();
            var user2 = TestUserRepository.GenerateUser("TESTUSER2", TimeTableRole.Management);

            userRepo.Users.Add(user);
            userRepo.Users.Add(user2);

            var bookingBlocking = new Booking
            {
                Classroom = "H.1.110",
                Week = 25,
                WeekDay = 2,
                StartBlock = 2,
                EndBlock = 4,
                Type = 0,
                Owner = user2.UserId,
            };
            bookingRepo.Bookings.Add(bookingBlocking);


            //When
            var booking = new Booking
            {
                Classroom = "H.1.110",
                Week = 25,
                WeekDay = 2,
                StartBlock = 1,
                EndBlock = 10,
                Type = BookingType.Maintenance,
                Owner = user.UserId,
            };
            var actualBookingResult = await managementService.MaintenanceBooking(classroomRepo, bookingRepo, userRepo,
                bookingService, booking, user.UserId, notifier);

            //Then
            Assert.AreEqual(BookingAvailability.Booked, actualBookingResult);

        }


    }
}

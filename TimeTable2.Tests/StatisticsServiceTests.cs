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
    public class StatisticsServiceTests
    {
        [TestMethod]
        public void AmountOfUsers()
        {
            //Init
            var repo = new TestUserRepository();
            var service = new StatisticsService();

            //When
            repo.Users.Add(TestUserRepository.GenerateUser());
            repo.Users.Add(TestUserRepository.GenerateUser());

            var amount = service.AmountOfUsers(repo);

            //Then
            Assert.AreEqual(2, amount, "Amount of users differs from expected");
        }

        [TestMethod]
        public void MostUsedClassroomByCourses()
        {
            //Init
            var repo = new TestClassroomRepository();
            var service = new StatisticsService();

            //When
            repo.Classrooms.Add(new Classroom
            {
                Id = Guid.NewGuid(),
                RoomId = "H.1.110",
                Courses = new List<Course>
                {
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Week = 22
                    },
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Week = 22
                    }
                    ,
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Week = 22
                    }
                },
            });
            repo.Classrooms.Add(new Classroom
            {
                Id = Guid.NewGuid(),
                RoomId = "H.1.112",
                Courses = new List<Course>
                {
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Week = 22
                    },
                    new Course
                    {
                        Id = Guid.NewGuid(),
                        Week = 22
                    }
                },
            });

            var rooms = service.GetMostUsedClassroomsByLessons(repo, 1, 22);

            //Then
            Assert.AreEqual(rooms.Count, 1, "More  or less than one room was returned");
            Assert.AreEqual(rooms[0].RoomId, "H.1.110", "Wrong room was returned");
        }

        [TestMethod]
        public void MostUsedClassroomByBookings()
        {
            //Init
            var repo = new TestClassroomRepository();
            var service = new StatisticsService();

            //When
            repo.Classrooms.Add(new Classroom
            {
                Id = Guid.NewGuid(),
                RoomId = "H.1.110",
                Bookings = new List<Booking>
                {
                    new Booking
                    {
                        Id = Guid.NewGuid(),
                        Week = 22
                    },
                    new Booking
                    {
                        Id = Guid.NewGuid(),
                        Week = 22
                    }
                    ,
                    new Booking
                    {
                        Id = Guid.NewGuid(),
                        Week = 22
                    }
                },
            });
            repo.Classrooms.Add(new Classroom
            {
                Id = Guid.NewGuid(),
                RoomId = "H.1.112",
                Bookings = new List<Booking>
                {
                    new Booking
                    {
                        Id = Guid.NewGuid(),
                        Week = 22
                    },
                    new Booking
                    {
                        Id = Guid.NewGuid(),
                        Week = 22
                    }
                },
            });

            var rooms = service.GetMostUsedClassroomsByBookings(repo, 1, 22);

            //Then
            Assert.AreEqual(rooms.Count, 1, "More  or less than one room was returned");
            Assert.AreEqual(rooms[0].RoomId, "H.1.110", "Wrong room was returned");
        }
    }
}

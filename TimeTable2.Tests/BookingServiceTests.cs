using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTable2.Engine;
using TimeTable2.Engine.Bookings;
using TimeTable2.Services;
using TimeTable2.Tests.Resources;

namespace TimeTable2.Tests
{
    [TestClass]
    public class BookingServiceTests
    {
        [TestMethod]
        public void TestAvailability()
        {
            var c = new Classroom()
            {
                RoomId = "H.1.110"
            };
            var classroomRepository = new TestClassroomRepository
            {
                //Given
                Courses = new List<Course>
                {
                    new Course()
                    {
                        Week = 1,
                        WeekDay = 1,
                        StartBlock = 2,
                        EndBlock = 5,
                    },
                    new Course()
                    {
                        Week = 1,
                        WeekDay = 1,
                        StartBlock = 7,
                        EndBlock = 9,
                    }
                }
            };
            var bookingRepository = new TestBookingRepository();

            var service = new BookingService(bookingRepository, classroomRepository);
            var existingLessons = classroomRepository.GetCoursesByRoomAndWeek("H.1.110", 1);
            
            //When
            var booking1 = GenerateBooking("H.1.110", 1, 1, 1, 1); // 1-1 out 2-5 and 7-9 -> 0
            var booking2 = GenerateBooking("H.1.110", 3, 5, 1, 1); // 3-5 in 2-5 and under 7-9 -> 1
            var booking3 = GenerateBooking("H.1.110", 3, 9, 1, 1); // 3-9 in 2-5 and 7-9 -> 2
            var booking4 = GenerateBooking("H.1.110", 8, 10, 1, 1); // 8-11 out 2-5 and in 7-9 -> 1

            //Then
            var availability1 = service.CourseAvailability(existingLessons, booking1);
            Assert.AreEqual(0, availability1.Count, "A1 failed");

            var availability2 = service.CourseAvailability(existingLessons, booking2);
            Assert.AreEqual(1, availability2.Count, "A2 failed");

            var availability3 = service.CourseAvailability(existingLessons, booking3);
            Assert.AreEqual(2, availability3.Count, "A3 failed");

            var availability4 = service.CourseAvailability(existingLessons, booking4);
            Assert.AreEqual(1, availability4.Count, "A4 failed");
        }

        public Booking GenerateBooking(string lokaalCode, int start, int end, int week, int weekday)
        {
            return new Booking()
            {
                Classroom = lokaalCode,
                StartBlock = start,
                EndBlock = end,
                Week = week,
                WeekDay = weekday
            };
        }
    }
}

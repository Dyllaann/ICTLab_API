using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Services
{
    public class BookingService
    {
        private IBookingRepository BookingRepository { get; }
        private IClassroomRepository ClassroomRepository { get; }

        #region CTOR
        public BookingService(IBookingRepository bookingRepository, IClassroomRepository classroomRepository)
        {
            BookingRepository = bookingRepository;
            ClassroomRepository = classroomRepository;
        }
        #endregion  

        public Booking BookRoom(Booking booking)
        {
            var logger = LogManager.GetLogger("BookingService");

            var existingLessons = ClassroomRepository.GetCoursesByRoomAndWeek(booking.Classroom, booking.Week);
            var blockingLessons = CourseAvailability(existingLessons, booking);

            if (blockingLessons.Count > 0)
            {
                logger.Info($"There was already a lesson planned during this time for this booking. {blockingLessons.Count}");
                return null;
            }

            var existingBookings = BookingRepository.GetBookingsByRoomAndWeek(booking.Classroom, booking.Week);
            var blockingBookings = BookingAvailability(existingBookings, booking);
            if (blockingBookings.Count > 0)
            {
                logger.Info($"There was already a booking planned during this time for this booking. {blockingBookings.Count}");
                return null;
            }

            


            return booking;
        }



        public List<Course> CourseAvailability(ICollection<Course> existingLessons, Booking booking)
        {
            var blocking = existingLessons.Where(l => l.WeekDay == booking.WeekDay 
                                                      //Starts inside another lesson
                                                      && (booking.StartBlock >= l.StartBlock && booking.StartBlock <= l.EndBlock
                                                     //Ends inside another lesson
                                                      || booking.EndBlock >= l.StartBlock && booking.EndBlock <= l.EndBlock
                                                      //Overlaps entire lesson
                                                      || booking.StartBlock <= l.StartBlock && booking.EndBlock >= l.EndBlock))
                                                    .ToList();


            return blocking;
        }
            
        public List<Booking> BookingAvailability(ICollection<Booking> existingLessons, Booking booking)
        {
            var blocking = existingLessons.Where(l => l.WeekDay == booking.WeekDay
                                                      //Starts inside another lesson
                                                      && (booking.StartBlock >= l.StartBlock && booking.StartBlock <= l.EndBlock
                                                          //Ends inside another lesson
                                                          || booking.EndBlock >= l.StartBlock && booking.EndBlock <= l.EndBlock
                                                          //Overlaps entire lesson
                                                          || booking.StartBlock <= l.StartBlock && booking.EndBlock >= l.EndBlock))
                .ToList();


            return blocking;
        }


    }
}

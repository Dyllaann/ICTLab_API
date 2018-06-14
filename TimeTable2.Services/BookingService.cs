using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using TimeTable2.Engine;
using TimeTable2.Engine.Bookings;
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

        #region Public Methods
        public BookingAvailability BookRoom(Booking booking, GoogleUserProfile user)
        {
            var logger = LogManager.GetLogger("BookingService");
            logger.Info($"Executing Booking request for UserId {user.UserId}");

            var existingLessons = ClassroomRepository.GetCoursesByRoomAndWeek(booking.Classroom, booking.Week);
            var blockingLessons = CourseAvailability(existingLessons, booking);

            logger.Info($"Booking: There are {blockingLessons.Count} blocking lessons");
            if (blockingLessons.Count > 0)
            {
                logger.Info($"There was already a lesson planned during this time for this booking. {blockingLessons.Count}");
                return Engine.Bookings.BookingAvailability.Scheduled;
            }


            var existingBookings = BookingRepository.GetBookingsByRoomAndWeek(booking.Classroom, booking.Week);
            var blockingBookings = BookingAvailability(existingBookings, booking);
            logger.Info($"Booking: There are {blockingBookings.Count} blocking bookings");

            if (blockingBookings.Count > 0)
            {
                logger.Info($"There was already a booking planned during this time for this booking. {blockingBookings.Count}");
                return Engine.Bookings.BookingAvailability.Booked;
            }

            logger.Info($"No blocking lessons or bookings. Creating new booking");
            booking.Owner = user.UserId;
            booking.Lokaal = ClassroomRepository.GetClassroomById(booking.Classroom);
            BookingRepository.CreateBooking(booking);
            logger.Info($"Booking success");
            return Engine.Bookings.BookingAvailability.Success;
        }

        public List<Booking> GetBookingsForUser(int week, string owner)
        {
            return BookingRepository.GetBookingFromUserByWeek(week, owner);
        }

        public List<FilterClassroom> Filter(int guests, int startBlock, int endBlock, int week, int weekDay)
        {
            var filteredRooms = new List<FilterClassroom>();

            var allRooms = ClassroomRepository.GetAllClassroomsWithCourses(week);
            foreach (var room in allRooms)
            {
                var block = CourseAvailability(room.Courses, week, startBlock, endBlock, weekDay);
                if (block.Count > 0)
                {
                    continue;
                }

                var classroom = new FilterClassroom(room);

                var nextLesson = room.Courses?.Where(c => c.StartBlock > endBlock).OrderBy(c => c.StartBlock)
                    .FirstOrDefault();

                classroom.FreeUntill = nextLesson?.EndBlock ?? 99;
                filteredRooms.Add(classroom);
            }

            return filteredRooms;
        }

        public bool DeleteBooking(Guid bookingId)
        {
            var logger = LogManager.GetLogger("BookingService");

            var booking = BookingRepository.GetBookingById(bookingId);
            if (booking == null)
            {
                logger.Warn($"No booking found for bookingId {bookingId}");
                return false;
            }
            BookingRepository.DeleteBooking(booking);
            return true;
        }

        #endregion

        #region Private Methods
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

        public List<Course> CourseAvailability(ICollection<Course> existingLessons, int week, int start, int end, int dayofweek)
        {
            var blocking = existingLessons.Where(l => l.Week == week
                                                      && l.WeekDay == dayofweek
                                                      //Starts inside another lesson
                                                      && (start >= l.StartBlock && start <= end
                                                          //Ends inside another lesson
                                                          || end >= l.StartBlock && end <= l.EndBlock
                                                          //Overlaps entire lesson
                                                          || start <= l.StartBlock && end >= l.EndBlock))
                .ToList();


            return blocking;
        }
        #endregion

    }
}

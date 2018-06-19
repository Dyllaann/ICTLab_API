using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using TimeTable2.Engine;
using TimeTable2.Engine.Bookings;
using TimeTable2.Engine.Management;
using TimeTable2.Notifier;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Services
{
    public class ManagementService
    {
        #region Public GET methods
        public List<User> GetAllUsers(IUserRepository userRepository)
        {
            return userRepository.GetAllUsers();
        }
        #endregion

        #region Public POST methods

        public bool EditUser(IUserRepository repository, UserEdit newUser)
        {
            var existingUser = repository.GetUserById(newUser.UserId);
            if (existingUser == null) return true;

            existingUser.Role = newUser.NewRole;
            existingUser.RoleString = newUser.NewRole.ToString();
            repository.UpdateUser(existingUser);
            return true;
        }
        public async Task<BookingAvailability> MaintenanceBooking(IClassroomRepository classroomRepository, IBookingRepository bookingRepository, IUserRepository userRepository,
            BookingService bookingService, Booking booking, string userId, INotifier notifier)
        {   
            var logger = LogManager.GetLogger("ManagementService");

            logger.Info($"Executing Booking request for UserId {userId}");

            var existingLessons = classroomRepository.GetCoursesByRoomAndWeek(booking.Classroom, booking.Week);
            var blockingLessons = bookingService.CourseAvailability(existingLessons, booking);

            logger.Info($"Booking: There are {blockingLessons.Count} blocking lessons");
            if (blockingLessons.Count > 0)
            {
                logger.Info($"There was already a lesson planned during this time for this booking. {blockingLessons.Count}");
                return BookingAvailability.Scheduled;
            }


            var existingBookings = bookingRepository.GetBookingsByRoomAndWeek(booking.Classroom, booking.Week);
            var blockingBookings = bookingService.BookingAvailability(existingBookings, booking);
            logger.Info($"Booking: There are {blockingBookings.Count} blocking bookings");

            if (blockingBookings.Count > 0)
            {
                var userlist = new List<string>();
                logger.Info($"There was already a booking planned during this time for this booking. {blockingBookings.Count}");
                foreach (var blockingBooking in blockingBookings)
                {
                    var bookingOwner = blockingBooking.Owner;
                    var bookingUser = userRepository.GetUserById(bookingOwner);
                    if (bookingUser.Role == TimeTableRole.Student)
                    {
                        bookingRepository.DeleteBooking(blockingBooking);
                        if (userlist.Contains(bookingOwner)) continue;
                        await notifier.Notify(bookingOwner, "Booking cancelled", $"One or more of your bookings have been cancelled due to maintenance in room {blockingBooking.Classroom}", "API");
                        userlist.Add(bookingOwner);
                    }
                }
            }

            existingBookings = bookingRepository.GetBookingsByRoomAndWeek(booking.Classroom, booking.Week);
            var blockingBookingsAfterRemoval = bookingService.BookingAvailability(existingBookings, booking);
            if (blockingBookingsAfterRemoval.Count > 0)
            {
                return BookingAvailability.Booked;
            }

            logger.Info($"No blocking lessons or bookings. Creating new booking");
            booking.Owner = userId;
            booking.Lokaal = classroomRepository.GetClassroomById(booking.Classroom);
            booking.Type = BookingType.Maintenance;
            bookingRepository.CreateBooking(booking);

            logger.Info($"Booking success");
            return BookingAvailability.Success;
        }
        #endregion

    }
}

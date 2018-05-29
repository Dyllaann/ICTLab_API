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

            var existingLessons = ClassroomRepository.GetCoursesByRoomAndWeek(booking.LokaalCode, booking.Week);
            var blocking = existingLessons.Where(l => l.WeekDay == booking.WeekDay
                                       && (l.StartBlock > booking.StartBlok
                                       && l.EndBlock > booking.EndBlok
                       
                                       || l.StartBlock < booking.StartBlok
                                       && l.EndBlock < booking.EndBlok)).ToList();
            if (blocking.Count > 0)
            {
                logger.Warn($"There was already a lesson planned during this time for this booking. {blocking.Count}");
                return null;
            }


            return booking;
        }



        public List<Course> BookingAvailability(ICollection<Course> existingLessons, Booking booking)
        {
            var blocking = existingLessons.Where(l => l.WeekDay == booking.WeekDay 
                                                      && ((booking.StartBlok >= l.StartBlock && booking.StartBlok <= l.EndBlock)
                                                      || (booking.EndBlok >= l.StartBlock && booking.EndBlok <= l.EndBlock)
                                                      || (booking.StartBlok <= l.StartBlock && booking.EndBlok >= l.EndBlock)))
                                                    .ToList();


            return blocking;
        }


    }
}

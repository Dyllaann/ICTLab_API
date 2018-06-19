using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Engine.Bookings;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Repository
{
    public class BookingRepository: DbRepository, IBookingRepository
    {
        public BookingRepository(DbContext context) : base(context) { }

        public List<Booking> GetBookingsByRoomAndWeek(string room, int week)
        {
            {
                var bookings = Context.Set<Booking>().Where(b => b.Week == week && b.Classroom == room).ToList();
                return bookings;
            }
        }

        public Booking CreateBooking(Booking booking)
        {
            Context.Set<Booking>().Add(booking);
            Context.SaveChanges();
            return booking;
        }

        public void DeleteBooking(Booking booking)
        {
            Context.Set<Booking>().Remove(booking);
            Context.SaveChanges();
        }

        public List<Booking> GetAllBookings()
        {
            return Context.Set<Booking>().ToList();
        }

        public List<Booking> GetBookingFromUserByWeek(int week, string owner)
        {
            return Context.Set<Booking>().Where(b => b.Week == week && b.Owner == owner).ToList();
        }

        public Booking GetBookingById(Guid id)
        {
            return Context.Set<Booking>().FirstOrDefault(b => b.Id == id);
        }

        public List<Booking> GetAllMaintenanceBookings(int week)
        {
            return Context.Set<Booking>().Where(b => b.Type == BookingType.Maintenance && b.Week == week).ToList();
        }

        public List<Booking> GetAllMaintenanceBookings()
        {
            return Context.Set<Booking>().Where(b => b.Type == BookingType.Maintenance).ToList();
        }

        public List<Booking> GetBookingsPerRoomPerWeek(string roomCode, int week)
        {
            return Context.Set<Booking>().Where(b => b.Classroom == roomCode && b.Week == week).ToList();
        }
    }
}

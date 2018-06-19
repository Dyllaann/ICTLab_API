using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Engine.Bookings;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Tests.Resources
{
    public class TestBookingRepository : IBookingRepository
    {
        public List<Booking> Bookings { get; set; }
        public TestBookingRepository(List<Booking> bookings)
        {
            Bookings = bookings;
        }

        public TestBookingRepository()
        {
            Bookings = new List<Booking>();
        }



        public List<Booking> GetBookingsByRoomAndWeek(string room, int week)
        {
            return Bookings.Where(b => b.Week == week && b.Classroom == room).ToList();
        }

        public Booking CreateBooking(Booking booking)
        {
            Bookings.Add(booking);
            return booking;
        }

        public void DeleteBooking(Booking booking)
        {
            Bookings.Remove(booking);
        }

        public List<Booking> GetAllBookings()
        {
            return Bookings;
        }

        public List<Booking> GetBookingFromUserByWeek(int week, string owner)
        {
            return Bookings.Where(b => b.Week == week && b.Owner == owner).ToList();
        }

        public Booking GetBookingById(Guid id)
        {
            return Bookings.FirstOrDefault(b => b.Id == id);
        }

        public List<Booking> GetAllMaintenanceBookings(int week)
        {
            return Bookings.Where(b => b.Type == BookingType.Maintenance && b.Week == week).ToList();
        }

        public List<Booking> GetAllMaintenanceBookings()
        {
            return Bookings.Where(b => b.Type == BookingType.Maintenance).ToList();
        }

        public List<Booking> GetBookingsPerRoomPerWeek(string roomCode, int week)
        {
            return Bookings.Where(b => b.Type == BookingType.Maintenance && b.Week == week).ToList();
        }
    }
}

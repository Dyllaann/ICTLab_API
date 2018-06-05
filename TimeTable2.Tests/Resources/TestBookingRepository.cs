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
        public List<Booking> GetBookingsByRoomAndWeek(string room, int week)
        {
            throw new NotImplementedException();
        }

        public Booking CreateBooking(Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}

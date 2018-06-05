using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Engine.Bookings;

namespace TimeTable2.Repository.Interfaces
{
    public interface IBookingRepository
    {
        List<Booking> GetBookingsByRoomAndWeek(string room, int week);
        Booking CreateBooking(Booking booking);
    }
}

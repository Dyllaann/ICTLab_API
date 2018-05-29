using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Repository
{
    public class BookingRepository: DbRepository, IBookingRepository
    {
        public BookingRepository(DbContext context) : base(context)
        {
        }



    }
}

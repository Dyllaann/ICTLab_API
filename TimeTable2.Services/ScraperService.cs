using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository;
using TimeTable2.Repository.Interfaces;
using TimeTable2.Scraper;

namespace TimeTable2.Services
{
    public class ScraperService
    {
        private IScraperRepository ScraperRepository { get; }
        private IClassroomRepository ClassroomRepository { get; }
        private IClassRepository ClassRepository { get; }
        private IBookingRepository BookingRepository { get; set; }

        public ScraperService(IScraperRepository scraperRepository, IClassroomRepository classroomRepository, IClassRepository classRepository, IBookingRepository bookingRepository)
        {
            ScraperRepository = scraperRepository;
            ClassroomRepository = classroomRepository;
            ClassRepository = classRepository;
            BookingRepository = bookingRepository;
        }


        public async Task<List<Classroom>> Scrape(int week)
        {
            var scraper = new WebScraper(ClassroomRepository, BookingRepository, ClassRepository, ScraperRepository);

            var roomsWithLessons = await scraper.Execute(week);

            if (roomsWithLessons == null)
            {
                return null;
            }

            ScraperRepository.AddOrUpdateClassrooms(roomsWithLessons);

            return roomsWithLessons;
        }
    }
}

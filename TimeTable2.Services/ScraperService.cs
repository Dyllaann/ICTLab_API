using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;
using TimeTable2.Scraper;

namespace TimeTable2.Services
{
    public class ScraperService
    {
        private IScraperRepository ScraperRepository { get; }
        private IClassroomRepository ClassroomRepository { get; }

        public ScraperService(IScraperRepository scraperRepository, IClassroomRepository classroomRepository)
        {
            ScraperRepository = scraperRepository;
            ClassroomRepository = classroomRepository;
        }


        public async Task<List<Classroom>> Scrape(int quarter, int week)
        {
            var scraper = new WebScraper();

            var roomsWithLessons = await scraper.Execute(ClassroomRepository, week);

            if (roomsWithLessons == null)
            {
                return null;
            }

            ScraperRepository.AddOrUpdateClassrooms(roomsWithLessons);

            return roomsWithLessons;
        }
    }
}

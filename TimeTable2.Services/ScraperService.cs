using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public ScraperService(IScraperRepository scraperRepository)
        {
            ScraperRepository = scraperRepository;
        }


        public List<Classroom> Scrape(int quarter, int week)
        {
            var scraper = new WebScraper();
            var listofrooms = ScraperRepository.GetAllClassrooms();

            var roomsWithLessons = scraper.Execute(listofrooms, quarter, week);

            ScraperRepository.AddOrUpdateClassrooms(roomsWithLessons);

            return roomsWithLessons;
        }
    }
}

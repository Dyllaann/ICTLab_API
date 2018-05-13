using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTable2.Engine;

namespace TimeTable2.Scraper
{
    public class WebScraper
    {
        /// <summary>
        /// Scrape the list of 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="quarter">The quarter of the year (1-4)</param>
        /// <param name="week">The week of the year (1-51)</param>
        /// <returns></returns>
        public List<Classroom> Execute(List<Classroom> filter, int quarter, int week)
        {
            /*var result = new Scraping
            {
                CreatedAt = DateTime.UtcNow
            };*/

            var browser = new HtmlWeb();
            var lokalen = new List<Classroom>();
            var i = 1;

            while (true)
            {
                var formattedNumber = i.ToString().PadLeft(5, '0');
                var url = $"http://misc.hro.nl/roosterdienst/webroosters/CMI/kw{quarter}/{week}/r/r{formattedNumber}.htm";

                var doc = browser.Load(url);

                var document = ReplaceLines(doc);
                var nodes = document.DocumentNode;

                var foundText = nodes.SelectNodes("//*[contains(., 'was not found on this server')]");
                if (foundText == null)
                {
                    //Get the actual room code as readable text
                    var roomNumberNode = nodes.SelectNodes("/html[1]/body[1]/center[1]/font[2]").FirstOrDefault();
                    var number = roomNumberNode?.InnerText;
                    var numberFiltered = number?.Substring(0, number.Length - 1);
                    var classroom = filter.FirstOrDefault(c => c.RoomId == numberFiltered);

                    //If it exists in the filter list
                    if (classroom != null)
                    {
                        //Get all the table ROWS for this room
                        var table = nodes.SelectNodes("/html[1]/body[1]/center[1]/table[1]/tr");

                        var blok = 1;
                        // j=2 to skip the first row as table headers (horizontal searching)
                        for (var j = 2; j <= table.Count; j++)
                        {
                            //Get all the columns PER row for the room
                            var tableColumns = nodes.SelectNodes($"/html[1]/body[1]/center[1]/table[1]/tr[{j}]/td");

                            if (tableColumns == null) //happens on days the room is completely free
                            {
                                continue;
                            }

                            var weekday = 0;
                            //For each column we have in the row (vertical searching)
                            foreach (var cell in tableColumns)
                            {

                                //Skip if this is the time indication cell
                                if (tableColumns.IndexOf(cell) == 0)
                                {
                                    weekday++;
                                    continue;
                                }

                                //Process all other cells
                                ProcessTableCell(cell, classroom, lokalen, weekday, blok, week);
                                weekday++;
                            }
                            blok++;
                        }
                    }
                    i++;
                }
                else
                {
                    return lokalen;
                }
            }
        }

        /// <summary>
        /// Process a cell inside the schedule table
        /// </summary>
        /// <param name="lessonColumn">The column the cell is in</param>
        /// <param name="lokaal">The room the scrape is for</param>
        /// <param name="lokalen">The (already existing) list of lessons</param>
        /// <param name="weekday">The identifier of the weekday (1-5)</param>
        /// <param name="j">The row the cell is in</param>
        /// <param name="week">The week of the year</param>
        private void ProcessTableCell(HtmlNode lessonColumn, Classroom lokaal, ICollection<Classroom> lokalen, int weekday, int j, int week)
        {
            //Count the rows of the table inside the cell
            var insideRows = lessonColumn.ChildNodes[0].ChildNodes.Count;
            //Count the elements of the content of the cell inside the table of the cell
            var innerRows = lessonColumn.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes.Count;


            //If we have information in the cell of the table and the block lasts the entire day
            //But we don't have three information cells inside the table
            //The day has to be something special.
            if (insideRows != 3 && innerRows == 2 && int.Parse(lessonColumn.Attributes["rowspan"].Value) / 2 == 15)
            {
                var data = lessonColumn.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes;
                var reason = data[0].InnerText;

                //If the classroom does not have any lessons in it, initialize the list
                if (lokaal.Courses == null)
                {
                    lokaal.Courses = new List<Course>();
                }

                //Try to find the course in the list to see if it exists
                var exists = lokaal.Courses.FirstOrDefault(c =>
                    c.WeekDay == weekday &&
                    c.Week == week);

                //If it already exists, take it and add it to the list.
                if (exists != null)
                {
                    lokaal.Courses.Add(exists);
                }
                else
                {
                    lokaal.Courses.Add(new Course
                    {
                        Id = Guid.NewGuid(),
                        WeekDay = weekday,
                        startBlok = 1,
                        EndBlok = 15,
                        VakCode = reason,
                        Week = week
                    });
                }
                var storedClassroom = lokalen.FirstOrDefault(k => k.RoomId == lokaal.RoomId);
                if (storedClassroom == null)
                {
                    lokalen.Add(lokaal);
                }
                else
                {
                    storedClassroom.Courses = lokaal.Courses;
                }
            }

            //If there are three information cells, and the first one has two children, there is a normal lesson this cell.
            if (lessonColumn.ChildNodes[0].ChildNodes.Count == 3 && innerRows == 2)
            {
                var data = lessonColumn.ChildNodes[0].ChildNodes;
                var startBlok = j;
                var eindBlok = startBlok + int.Parse(lessonColumn.Attributes["rowspan"].Value) / 2 - 1;
                var klas = data[0].InnerText.Replace(" ", "").Replace(".", "");
                var docent = data[1].InnerText.Replace(" ", "");
                var cursus = data[2].InnerText.Replace(" ", "");

                if (lokaal.Courses == null)
                {
                    lokaal.Courses = new List<Course>();
                }

                var exists = lokaal.Courses.FirstOrDefault(c =>
                    c.WeekDay == weekday &&
                    c.startBlok == startBlok &&
                    c.EndBlok == eindBlok &&
                    c.Week == week);

                if (exists != null)
                {
                    lokaal.Courses.Add(exists);
                }
                else
                {

                    lokaal.Courses.Add(new Course()
                    {
                        WeekDay = weekday,
                        startBlok = startBlok,
                        Docent = docent,
                        EndBlok = eindBlok,
                        Klas = klas,
                        VakCode = cursus,
                        Id = Guid.NewGuid(),
                        Week = week
                    });

                }
                var storedClassroom = lokalen.FirstOrDefault(k => k.RoomId == lokaal.RoomId);
                if (storedClassroom == null)
                {
                    lokalen.Add(lokaal);
                }
                else
                {
                    storedClassroom.Courses = lokaal.Courses;
                }
            }

            //If we have two information rules, and the first cell contains two rows, the lesson is a lesson not specifically for a class.
            if (lessonColumn.ChildNodes[0].ChildNodes.Count == 2 && innerRows == 2)
            {
                var data = lessonColumn.ChildNodes[0].ChildNodes;
                var startBlok = j;
                var eindBlok = startBlok + int.Parse(lessonColumn.Attributes["rowspan"].Value) / 2 - 1;
                var klas = data[0].InnerText.Replace(" ", "").Replace(".", "");
                var cursus = data[1].InnerText.Replace(" ", "");

                if (lokaal.Courses == null)
                {
                    lokaal.Courses = new List<Course>();
                }

                var exists = lokaal.Courses.FirstOrDefault(c =>
                    c.WeekDay == weekday &&
                    c.startBlok == startBlok &&
                    c.EndBlok == eindBlok &&
                    c.Week == week);

                if (exists != null)
                {
                    lokaal.Courses.Add(exists);
                }
                else
                {
                    lokaal.Courses.Add(new Course()
                    {
                        WeekDay = weekday,
                        startBlok = startBlok,
                        EndBlok = eindBlok,
                        Klas = klas,
                        VakCode = cursus,
                        Id = Guid.NewGuid(),
                        Week = week
                    });
                }

                var storedClassroom = lokalen.FirstOrDefault(k => k.RoomId == lokaal.RoomId);
                if (storedClassroom == null)
                {
                    lokalen.Add(lokaal);
                }
                else
                {
                    storedClassroom.Courses = lokaal.Courses;
                }
            }
        }

        /// <summary>
        /// Replace all the unneccesarry newlines in a string
        /// </summary>
        /// <param name="doc">HtmlDocument to turn into a new one without newlines</param>
        /// <returns></returns>
        public HtmlDocument ReplaceLines(HtmlDocument doc)
        {
            var html = doc.ParsedText;
            var newHtml = html.Replace("\r\n", string.Empty).Replace("\"", "\"");

            var document = new HtmlDocument();
            document.LoadHtml(newHtml);
            return document;
        }

    }
}

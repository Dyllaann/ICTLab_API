using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable2.Engine;

namespace TimeTable2.Scraper
{
    public class WebScraper
    {
        public List<string> Execute(List<string> filter)
        {
            var browser = new HtmlWeb();
            List<string> rooms = new List<string>();
            List<Course> lessen = new List<Course>();
            var i = 1;

            var finished = false;
            while (!finished)
            {
                var formattedNumber = i.ToString().PadLeft(5, '0');
                var url = $"http://misc.hro.nl/roosterdienst/webroosters/CMI/kw4/17/r/r{formattedNumber}.htm";

                var doc = browser.Load(url);

                var document = ReplaceLines(doc);
                var documentNode = document.DocumentNode;

                var foundText = documentNode.SelectNodes("//*[contains(., 'was not found on this server')]");
                if(foundText == null)
                {
                    var roomNumberNode = documentNode.SelectNodes("/html[1]/body[1]/center[1]/font[2]").FirstOrDefault();
                    var number = roomNumberNode?.InnerText;
                    var numberFiltered = number?.Substring(0, number.Length - 1);


                    i++;
                }
                else
                {
                    finished = true;
                    return rooms;
                }

            }
            return rooms;
        }

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

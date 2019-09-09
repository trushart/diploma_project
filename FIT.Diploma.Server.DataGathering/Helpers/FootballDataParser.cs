using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace FIT.Diploma.Server.DataGathering.Helpers
{
    public class FootballDataParser
    {
        private string domein = "http://www.football-data.co.uk";
        private string spainLeagueArchives = "/spainm.php";

        public string GetDomain => domein;

        //get all links that contains some string (filter)
        public List<string> GetAllLinks(string filter)
        {
            List<string> result = new List<string>();

            //get web page content
            WebRequest request = WebRequest.Create(domein + spainLeagueArchives);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();

            //load html response, for parsing
            HtmlDocument document = new HtmlDocument();
            document.Load(data);
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];
                if (att.Value.Contains(filter))
                    result.Add(att.Value);
            }
            return result;
        }
    }
}

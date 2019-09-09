using FIT.Diploma.Server.DataGathering.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataGathering.Helpers
{
    public class BwinHtmlParser
    {
        #region Singleton
        private static volatile BwinHtmlParser currentInstance;
        private static object syncRoot = new Object();

        public static BwinHtmlParser Instance
        {
            get
            {
                if (currentInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (currentInstance == null)
                            currentInstance = new BwinHtmlParser();
                    }
                }

                return currentInstance;
            }
        }
        #endregion

        private class OddItem
        {
            public string OddCategory { get; set; }
            public string OddName { get; set; }
            public double OddCoef { get; set; }
        }

        public string GetMatchDetailsUrl(string matchHtmlSource)
        {
            string result = string.Empty;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(matchHtmlSource);
            var divWithLinks = doc.DocumentNode.Descendants("div")
                                .Where(d =>
                                   d.Attributes.Contains("class")
                                   &&
                                   d.Attributes["class"].Value.Equals("mb-event-details-buttons")
                                );
            if(divWithLinks.Count() > 1)
            {
                //to do log: should be 0 or 1
            }
            var divWithLink = divWithLinks.FirstOrDefault();

            if (divWithLink != null)
            {
                HtmlNode link = divWithLink.SelectNodes("//a[@href]").FirstOrDefault();
                if (link != null)
                    result = link.GetAttributeValue("href", string.Empty);
            }
            return result;
        }

        public BwinOddsModel ParseOdds(string htmlContent)
        {
            BwinOddsModel result = new BwinOddsModel();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            //team names
            var matchTitle = doc.GetElementbyId("event-info")?.SelectSingleNode("h1")?.InnerText;
            ParseTeamNames(matchTitle, ref result);

            //main results
            ParseAndSafeOdds(doc, "Match Result", ref result, ParseMainResults);

            //DoubleChances
            ParseAndSafeOdds(doc, "Double Chance", ref result, ParseDoubleChances);

            //BothTeamsToScore
            ParseAndSafeOdds(doc, "Both Teams to Score", ref result, ParseBothTeamsToScore);

            //Total2_5
            ParseAndSafeOdds(doc, "Total Goals - Over/Under", ref result, ParseTotals);

            //half-time main results
            ParseAndSafeOdds(doc, "Half Time result", ref result, ParseHalfTimeMainResults);

            return result;
        }

        private delegate bool MethodNameDelegate(List<OddItem> odds, ref BwinOddsModel model);

        private void ParseAndSafeOdds(HtmlDocument doc, string oddCategory, ref BwinOddsModel result, MethodNameDelegate func )
        {
            var nodeWithResults = GetGroupOfOneeOddType(doc, oddCategory);
            if(nodeWithResults != null)
            {
                var odds = GetAllPossibleOdds(nodeWithResults, oddCategory);
                if (!func(odds, ref result))
                    Console.WriteLine($"[ParseAndSafeOdds] Failed to parse '{oddCategory}' odds ('{nodeWithResults?.InnerText}')");
            }
            else
                Console.WriteLine($"[ParseAndSafeOdds] Failed to parse '{oddCategory}' odds. Node with this category couldn't be found");
        }

        private List<OddItem> GetAllPossibleOdds (HtmlNode node, string oddCategory)
        {
            List<OddItem> result = new List<OddItem>();
            HtmlNode table = node.SelectNodes("div//table//tbody")?.FirstOrDefault();
            foreach (HtmlNode tr in table?.SelectNodes("tr")) {
                foreach (HtmlNode td in tr?.SelectNodes("td"))
                {
                    var buttonDivs = td.SelectSingleNode("button")?.SelectNodes("div");
                    string oddName = buttonDivs?.FirstOrDefault()?.InnerText;
                    string oddCoefTxt = buttonDivs?.LastOrDefault()?.InnerText;
                    double oddCoef;
                    if(Double.TryParse(oddCoefTxt, out oddCoef))
                    {
                        result.Add(new OddItem {
                            OddCategory = oddCategory,
                            OddName = oddName,
                            OddCoef = oddCoef
                        });
                    }
                    else
                    {
                        Console.WriteLine($"[GetAllPossibleOdds] Couldn't parse odd coef ({oddCoefTxt})");
                    }
                }
            }
            return result;
        }

        private HtmlNode GetGroupOfOneeOddType(HtmlDocument doc, string oddType)
        {
            var result = doc.DocumentNode.Descendants("div")
                                .Where(d =>
                                   d.Attributes.Contains("class")
                                   &&
                                   d.Attributes["class"].Value.Equals("marketboard-event-with-header")       
                                   &&
                                   d.Descendants("span").FirstOrDefault().InnerText == oddType
                                );
            if(result.Count() != 1)
            {
                //to do log
                Console.WriteLine("[GetGroupOfOneeOddType] Found more or less then 1 element for odd type: {0}", oddType);
            }
            return result.FirstOrDefault();
        }

        private bool ParseTeamNames(string matchTitle, ref BwinOddsModel model)
        {
            var teams = matchTitle.Trim().Split('-');
            if(teams.Count() != 2)
            {
                //to do log
                Console.WriteLine("Problem with parsing team names");
                return false;
            }
            model.HomeTeam = teams[0].Trim();
            model.AwayTeam = teams[1].Trim();
            return true;
        }

        private bool ParseTotals(List<OddItem> odds, ref BwinOddsModel result)
        {
            bool ret = true;
            if (odds.Count >= 2)
            {
                foreach (var odd in odds)
                {
                    var oddName = odd.OddName.Trim();
                    if (oddName.Equals("Over 2,5"))
                    {
                        result.Total2_5Over = odd.OddCoef;
                    }
                    else if (oddName.Equals("Under 2,5"))
                    {
                        result.Total2_5Under = odd.OddCoef;
                    }
                    else
                    {
                        //for now ignore other totals - only 2.5 is required
                    }
                }
            }
            else ret = false;
            return ret;
        }

        private bool ParseBothTeamsToScore(List<OddItem> odds, ref BwinOddsModel result)
        {
            bool ret = true;
            if (odds.Count == 2)
            {
                foreach (var odd in odds)
                {
                    var oddName = odd.OddName.Trim();
                    if (oddName.Equals("Yes"))
                    {
                        result.BothTeamsToScore_Yes = odd.OddCoef;
                    }
                    else if (oddName.Equals("No"))
                    {
                        result.BothTeamsToScore_No = odd.OddCoef;
                    }
                    else
                    {
                        Console.WriteLine($"[ParseDoubleChances] Unknown odd name ('{oddName}')");
                        ret = false;
                    }
                }
            }
            else ret = false;
            return ret;
        }

        private bool ParseDoubleChances(List<OddItem> odds, ref BwinOddsModel result)
        {
            bool ret = true;
            if (odds.Count == 3)
            {
                foreach (var odd in odds)
                {
                    var oddName = odd.OddName.Trim();
                    if (oddName.Contains(result.HomeTeam) && oddName.Contains("or") && oddName.Contains("X"))
                    {
                        result.DoubleChanceCoef_1X = odd.OddCoef;
                    }
                    else if (oddName.Contains(result.AwayTeam) && oddName.Contains("or") && oddName.Contains("X"))
                    {
                        result.DoubleChanceCoef_X2 = odd.OddCoef;
                    }
                    else if (oddName.Contains(result.HomeTeam) && oddName.Contains("or") && oddName.Contains(result.AwayTeam))
                    {
                        result.DoubleChanceCoef_12 = odd.OddCoef;
                    }
                    else
                    {
                        Console.WriteLine($"[ParseDoubleChances] Unknown odd name ('{oddName}')");
                        ret = false;
                    }
                }
            }
            else ret = false;
            return ret;
        }

        private bool ParseHalfTimeMainResults(List<OddItem> odds, ref BwinOddsModel result)
        {
            bool ret = true;
            if (odds.Count == 3)
            {
                foreach (var odd in odds)
                {
                    if (odd.OddName.Trim() == result.HomeTeam.Trim())
                    {
                        result.HTHomeWinCoef = odd.OddCoef;
                    }
                    else if (odd.OddName.Trim() == result.AwayTeam.Trim())
                    {
                        result.HTAwayWinCoef = odd.OddCoef;
                    }
                    else if (odd.OddName.Trim() == "X")
                    {
                        result.HTDrawCoef = odd.OddCoef;
                    }
                    else
                    {
                        Console.WriteLine($"[ParseMainResults] Unknown odd name ('{odd.OddName.Trim()}')");
                        ret = false;
                    }
                }
            }
            else ret = false;
            return ret;
        }

        private bool ParseMainResults(List<OddItem> odds, ref BwinOddsModel result)
        {
            bool ret = true;
            if (odds.Count == 3)
            {
                foreach (var odd in odds)
                {
                    if (odd.OddName.Trim() == result.HomeTeam.Trim())
                    {
                        result.HomeWinCoef = odd.OddCoef;
                    }
                    else if (odd.OddName.Trim() == result.AwayTeam.Trim())
                    {
                        result.AwayWinCoef = odd.OddCoef;
                    }
                    else if (odd.OddName.Trim() == "X")
                    {
                        result.DrawCoef = odd.OddCoef;
                    }
                    else
                    {
                        Console.WriteLine($"[ParseMainResults] Unknown odd name ('{odd.OddName.Trim()}')");
                        ret = false;
                    }
                }
            }
            else ret = false;
            return ret;
        }
    }
}

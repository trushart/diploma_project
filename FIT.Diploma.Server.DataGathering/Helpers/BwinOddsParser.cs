using System;
using System.Collections.Generic;
using OpenQA.Selenium.PhantomJS;
using System.Text.RegularExpressions;
using System.Net;
using System.Globalization;
using System.Threading;
using System.IO;
using FIT.Diploma.Server.DataGathering.Models;
using FIT.Diploma.Server.DataAccess.Repositories;

namespace FIT.Diploma.Server.DataGathering.Helpers
{
    public class BwinOddsParser : IDisposable
    {
        private string domein = "https://sports.bwin.com";
        private string laLigaUrl = "/en/sports#leagueIds=16108&sportId=4";
        private PhantomJSDriver _driver = null;
        private PhantomJSDriver Driver
        {
            get
            {
                if (_driver == null) _driver = InitializePhantomJSDriver();
                return _driver;
            }
        }

        private PhantomJSDriver InitializePhantomJSDriver()
        {
            var service = PhantomJSDriverService.CreateDefaultService();
            service.SslProtocol = "tlsv1";

            return new PhantomJSDriver(service);
        }

        public void Dispose()
        {
            Console.WriteLine("*****Dispose BwinOddsParser*****");
            Driver.Quit();
            _driver = null;
        }

        public List<string> GetAndSaveAllAvailableGameLinks()
        {
            List<string> result = new List<string>();

            Driver.Navigate().GoToUrl(domein + laLigaUrl);

            var allMatchesDivs = Driver.FindElementsByClassName("marketboard-event-group__item--event");

            Console.WriteLine("Number of available matches: " + allMatchesDivs.Count);
            foreach(var match in allMatchesDivs)
            {
                var link = BwinHtmlParser.Instance.GetMatchDetailsUrl(match.GetAttribute("outerHTML"));
                if (IsLinkRelevant(link)) result.Add(link);
            }            
            return result;
        }

        private bool IsLinkRelevant(string link)
        {
            string pattern = @"^/en/sports/events/.*betting.*$";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            return r.IsMatch(link);
        }

        public bool ParseAndSaveGameOdds(string link)
        {
            var result = false;
            DateTime? dateTime = null;
            Driver.Navigate().GoToUrl(domein + link);

            //get date and time
            try
            {
                var gameDateTimeTxt = RuntRetriableFunc(() => Driver.FindElementByCssSelector("span.event-block__start-date").GetAttribute("innerHTML"), 3);
                gameDateTimeTxt = WebUtility.HtmlDecode(gameDateTimeTxt);
                dateTime = ParseDateTime(gameDateTimeTxt);
                if (dateTime == null)
                    throw new Exception("Probably bad format of date and time. Text = " + gameDateTimeTxt);
            }
            catch(Exception ex)
            {
                //to do log: couldn't parse dateTime of game
                Console.WriteLine("Couldn't parse dateTime of game. Exception: {0}", ex.Message);
            }

            //get all odds html - then parse (only one request to bookmaker server)
            try
            {
                var clickToAll = RuntRetriableFunc(() => {
                    var allOddsLink = Driver.FindElementByCssSelector("#event-horizontal-nav > div > div.nav.has-ellipsis.nav-pills.nav--has5 > div:nth-child(1) > a");
                    if (allOddsLink != null)
                    {
                        allOddsLink.Click();
                        Thread.Sleep(3000);
                    }
                    else throw new Exception("link wasn't found. Should retry search.");
                    return 1;
                }, 3);
                if (clickToAll == 0)
                    Console.WriteLine("Problem with clicking on All link");
                var htmlContent = RuntRetriableFunc(() => {
                    return Driver.FindElementByCssSelector("#bet-offer").GetAttribute("outerHTML");
                    }, 3);

                //save context to file
                //var fileName = link.Replace('/', '_');
                //File.WriteAllText("D:\\bwin\\" + fileName + ".txt", htmlContent);

                //parse html content
                BwinHtmlParser parser = new BwinHtmlParser();
                var odds = parser.ParseOdds(htmlContent);                

                //save parsed odds to DB
                result |= SaveBwinOddsModelToDb(odds, dateTime.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't get game all odds HTML. Exception: {0}", ex.Message);
                Driver.Navigate().Refresh();
            }
            return result;
        }

        private T RuntRetriableFunc<T>(Func<T> func, int maxTries)
        {
            T result = default(T);
            for(int i = 0; i < maxTries; i++)
            {
                try
                {
                    result = func.Invoke();
                    break;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("[RuntRetriableFunc] Try = {0}, MaxTries = {1}. Exception: {2}", i, maxTries, ex.Message);                    
                }
            }
            return result;
        }

        private bool SaveBwinOddsModelToDb(BwinOddsModel odds, DateTime matchDate)
        {
            bool onlyCreated;
            var repo = new LeagueDataRepository();
            var bookmakerRepo = new BookmakerOddsDataRepository();
            var matchDb = repo.GetGame(odds.HomeTeam, odds.AwayTeam, matchDate, out onlyCreated, "bwin");

            var bookmaker = bookmakerRepo.GetBookmaker("Bet&Win");
            var bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = odds.HomeWinCoef;
                bookmakerOdd.DrawCoef = odds.DrawCoef;
                bookmakerOdd.AwayWinCoef = odds.AwayWinCoef;

                bookmakerOdd.BothTeamsToScore_No = odds.BothTeamsToScore_No;
                bookmakerOdd.BothTeamsToScore_Yes = odds.BothTeamsToScore_Yes;

                bookmakerOdd.Total2_5Over = odds.Total2_5Over;
                bookmakerOdd.Total2_5Under = odds.Total2_5Under;

                bookmakerOdd.HTHomeWinCoef = odds.HTHomeWinCoef;
                bookmakerOdd.HTDrawCoef = odds.HTDrawCoef;
                bookmakerOdd.HTAwayWinCoef = odds.HTAwayWinCoef;

                bookmakerOdd.DoubleChanceCoef_12 = odds.DoubleChanceCoef_12;
                bookmakerOdd.DoubleChanceCoef_1X = odds.DoubleChanceCoef_1X;
                bookmakerOdd.DoubleChanceCoef_X2 = odds.DoubleChanceCoef_X2;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }
            return onlyCreated;
        }

        private DateTime? ParseDateTime(string dateTime)
        {
            DateTime? result = null;
            CultureInfo provider = new CultureInfo("en-US");

            string format = "M/d/yyyy, h:mm tt";
            try
            {
                result = DateTime.ParseExact(dateTime, format, provider, DateTimeStyles.AllowWhiteSpaces);
                Console.WriteLine("{0} converts to {1}.", dateTime, result.ToString());
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format [{1}].", dateTime, format);
            }
            return result;
        }
    }
}

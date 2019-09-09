using FIT.Diploma.Server.DataGathering.Helpers;
using FIT.Diploma.Server.DataGathering;
using System;
using System.IO;
using System.Threading;
using FIT.Diploma.Server.DataAnalysis;
using FIT.Diploma.REST.Host.TaskExecutors;
using FIT.Diploma.REST.Client;
using System.Text.RegularExpressions;
using FIT.Diploma.Shared.DataAccess.Dto;

namespace FIT.Diploma.Server.ConsoleApp.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestFileLoader();
            //TestWebParser();
            //TestFootballDataManager();
            //TestBwinParser();
            //TestBwinHtmlParser();
            //TestRepo();
            //TestStandingTableAnalyzer();
            //TestCollector();
            //TestSeasonStatsAnalyzer();
            TestRoundsStatsAnalyzer();
            //TestHeadToHeadStatsAnalyzer();
            //TestFootballTeamFormAnalyzer();
            //TestBookmakerOddsStatsAnalyzer();
            //TestAverageRoundStatsAnalyzer();
            //TestRestClient();
            //TestUpdateEndPointWithParams();
            //TestRestClientPost();
            //TestRestClientPredictions();
            //TestGamePrediction();
            Console.ReadKey();
        }

        static void TestGamePrediction()
        {
            var scheduler = new GamePredictionAnalyzer();
            scheduler.RunAnalyzing();
        }

        static void TestRestClientPredictions()
        {
            var client = new RestClient();
            var currentPredictions = client.GetCurrentSeasonPredictions(4);
            Console.WriteLine($"currentPredictions count: {currentPredictions.Predictions.Count}");

            var allPredictions = client.GetAllSeasonPredictions(4);
            Console.WriteLine($"allPredictions count: {allPredictions.Predictions.Count}");

            var finishedPredictions = client.GetFinishedSeasonPredictions(4);
            Console.WriteLine($"finishedPredictions count: {finishedPredictions.Predictions.Count}");
        }

        static void TestRestClientPost()
        {
            var client = new RestClient();
            var condition = new SearchToolConditionsDto
            {
                BothTeamsScore = true,
                TeamId = 16,
                GameTotal = new ST_Total
                {
                    GoalsNumber = 2,
                    TotalType = ST_TotalType.Over
                },
                TimeRange = new SearchTimeRangeDto
                {
                    FromDate = new DateTime(2013, 1, 1),
                    ToDate = DateTime.Now
                }
            };
            var leagues = client.GetNumberAllGames(condition);
        }

        private static string UpdateEndPointWithParams(string endpoint, params string[] parameters)
        {
            var numberOfParams = parameters?.Length ?? 0;
            var regex = new Regex("{[a-zA-Z0-9]+}");
            if (regex.Matches(endpoint).Count != numberOfParams)
                throw new Exception("Number of parameters not matches");
            if (numberOfParams == 0)
                return endpoint;
            foreach(var param in parameters)
            {
                endpoint = regex.Replace(endpoint, param, 1);
            }
            return endpoint;
        }

        static void TestUpdateEndPointWithParams()
        {
            var endpoint1 = "/leagues/info/all";
            string [] params1 = null;
            var result1 = UpdateEndPointWithParams(endpoint1, params1);
            Console.WriteLine($"Original endpoint: '{endpoint1}'  Updated: '{result1}'");

            var endpoint2 = "/leagues/info/{leagueId}";
            var result2 = UpdateEndPointWithParams(endpoint2, "101");
            Console.WriteLine($"Original endpoint: '{endpoint2}'  Updated: '{result2}'");
                        
            var endpoint3 = "/seasons/{seasonId}/h2h/{team1Id}/{team2Id}";
            var result3 = UpdateEndPointWithParams(endpoint3, "season#1", "team#Arsenal", "team#RealMadrid");
            Console.WriteLine($"Original endpoint: '{endpoint3}'  Updated: '{result3}'");
        }

        static void TestRestClient()
        {
            var client = new RestClient();
            var leagues = client.GetAllLeagues();
        }

        static void TestRepo()
        {
            var scheduler = new SchedulerForBwin();
            scheduler.Execute();
        }

        static void TestCollector() {
            var collector = new GameStatsCollector();
            Console.WriteLine("Start BookmakerOddsCollector.");
            collector.Start();
            Thread.Sleep(1000 * 60 * collector.GetTimeout());
            collector.Stop();
            Console.WriteLine("Stop BookmakerOddsCollector.");
        }

        static void TestFootballDataManager()
        {
            FootballDataManager manager = new FootballDataManager();
            manager.ProcessAllLinks();
        }

        static void TestSeasonStatsAnalyzer()
        {
            var analyzer = new SeasonStatsAnalyzer();
            Console.WriteLine("Start SeasonStatsAnalyzer.");
            analyzer.RunAnalyzing();
            Console.WriteLine("Stop SeasonStatsAnalyzer.");
        }

        static void TestAverageRoundStatsAnalyzer()
        {
            var analyzer = new AverageRoundStatsAnalyzer();
            Console.WriteLine("Start AverageRoundStatsAnalyzer.");
            analyzer.RunAnalyzing();
            Console.WriteLine("Stop AverageRoundStatsAnalyzer.");
        }

        static void TestBookmakerOddsStatsAnalyzer()
        {
            var analyzer = new BookmakerOddsStatsAnalyzer();
            Console.WriteLine("Start BookmakerOddsStatsAnalyzer.");
            analyzer.RunAnalyzing();
            Console.WriteLine("Stop BookmakerOddsStatsAnalyzer.");
        }

        static void TestFootballTeamFormAnalyzer()
        {
            var analyzer = new FootballTeamFormAnalyzer();
            Console.WriteLine("Start FootballTeamFormAnalyzer.");
            analyzer.RunAnalyzing();
            Console.WriteLine("Stop FootballTeamFormAnalyzer.");
        }

        static void TestRoundsStatsAnalyzer()
        {
            var analyzer = new RoundsStatsAnalyzer();
            Console.WriteLine("Start RoundsStatsAnalyzer.");
            analyzer.RunAnalyzing();
            Console.WriteLine("Stop RoundsStatsAnalyzer.");
        }

        static void TestHeadToHeadStatsAnalyzer()
        {
            var analyzer = new HeadToHeadStatsAnalyzer();
            Console.WriteLine("Start HeadToHeadStatsAnalyzer.");
            analyzer.RunAnalyzing();
            Console.WriteLine("Stop HeadToHeadStatsAnalyzer.");
        }


        static void TestStandingTableAnalyzer()
        {
            var analyzer = new StandingTableAnalyzer();
            Console.WriteLine("Start StandingTableAnalyzer.");
            analyzer.RunAnalyzing();
            Console.WriteLine("Stop StandingTableAnalyzer.");
        }

        static void TestBookmakerOddsCollector()
        {
            BookmakerOddsCollector collector = new BookmakerOddsCollector();
            Console.WriteLine("Start BookmakerOddsCollector.");
            collector.Start();
            Thread.Sleep(1000 * 60 * collector.GetTimeout());
            collector.Stop();
            Console.WriteLine("Stop BookmakerOddsCollector.");
        }

        static void TestBwinParser()
        {
            BwinOddsParser parser = new BwinOddsParser();
            Console.WriteLine("Start BwinParser.");
            var allLinks = parser.GetAndSaveAllAvailableGameLinks();
            foreach(var link in allLinks)
            {
                Console.WriteLine("Found link: " + link);
                parser.ParseAndSaveGameOdds(link);
            }
            Console.WriteLine("Stop BwinParser.");
        }

        static void TestBwinHtmlParser()
        {
            BwinHtmlParser parser = new BwinHtmlParser();
            Console.WriteLine("Start BwinHtmlParser.");
            var htmlContent = File.ReadAllText("D:\\bwin\\_en_sports_events_4_16108_6456879_betting_celta-de-vigo-girona.txt");
            var odds = parser.ParseOdds(htmlContent);
            Console.WriteLine("Stop BwinHtmlParser.");
        }

        static void TestWebParser()
        {
            int counter = 0;
            FootballDataParser parser = new FootballDataParser();
            Console.WriteLine("Start WebParser.");
            foreach (var link in parser.GetAllLinks("SP1.csv"))
            {
                Console.WriteLine("link [{0}]: {1}", counter++, link);
            }
            Console.WriteLine("Stop WebParser.");
        }

        static void TestFileLoader()
        {
            string dataSourceUrl = "http://www.football-data.co.uk";
            string fileLink = "/mmz4281/1617/SP1.csv";
            string fileLinkAbsolute = dataSourceUrl + fileLink;
            DataSourceFileLoader loader = new DataSourceFileLoader(dataSourceUrl);
            if (loader.DownloadFile(fileLink, "temp.csv"))
                Console.WriteLine("Downloading file {0} succeed", fileLinkAbsolute);
            else Console.WriteLine("Downloading file {0} failes", fileLinkAbsolute);
        }
    }
}

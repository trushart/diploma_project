using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.LeagueData;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using DbGameResult = FIT.Diploma.Server.Database.LeagueData.GameResult;

namespace FIT.Diploma.Server.DataAnalysis.SearchingTool.Tests
{
    [TestFixture]
    public class SearchEngineTests
    {
        private Dictionary<string, int> Teams = new Dictionary<string, int>
        {
            {"Arsenal FC", 1 },
            {"Barsa", 2 },
            {"Real Madrid", 3 },
            {"Spartak M", 4 },
            {"Atletico", 5 },
            {"Chelsea", 6 },
            {"MU", 7 },
        };

        private List<Game> ReturnGames()
        {
            List<Game> resultList = new List<Game>();
            int gameId = 1;
            int dayId = 1;

            //Arsenal - Barsa 2-1 (2017-03-01)
            resultList.Add(new Game
            {
                GameId = gameId++,
                HomeTeamId = Teams["Arsenal FC"],
                AwayTeamId = Teams["Barsa"],
                Date = new DateTime(2017, 3, dayId++),
                HomeTeamGoals = 2,
                AwayTeamGoals = 1,
                Result = DbGameResult.HomeWin
            });

            //Arsenal - MU 1-0 (2017-03-02)
            resultList.Add(new Game
            {
                GameId = gameId++,
                HomeTeamId = Teams["Arsenal FC"],
                AwayTeamId = Teams["MU"],
                Date = new DateTime(2017, 3, dayId++),
                HomeTeamGoals = 1,
                AwayTeamGoals = 0,
                Result = DbGameResult.HomeWin
            });

            //Real Madrid - Arsenal 1-1 (2017-03-03)
            resultList.Add(new Game
            {
                GameId = gameId++,
                HomeTeamId = Teams["Real Madrid"],
                AwayTeamId = Teams["Arsenal FC"],
                Date = new DateTime(2017, 3, dayId++),
                HomeTeamGoals = 1,
                AwayTeamGoals = 1,
                Result = DbGameResult.Draw
            });

            //Spatak M - Arsenal 4-1 (2017-03-04)
            resultList.Add(new Game
            {
                GameId = gameId++,
                HomeTeamId = Teams["Spartak M"],
                AwayTeamId = Teams["Arsenal FC"],
                Date = new DateTime(2017, 3, dayId++),
                HomeTeamGoals = 4,
                AwayTeamGoals = 1,
                Result = DbGameResult.HomeWin
            });

            //Atletico - Arsenal 0-0 (2017-03-05)
            resultList.Add(new Game
            {
                GameId = gameId++,
                HomeTeamId = Teams["Atletico"],
                AwayTeamId = Teams["Arsenal FC"],
                Date = new DateTime(2017, 3, dayId++),
                HomeTeamGoals = 0,
                AwayTeamGoals = 0,
                Result = DbGameResult.Draw
            });

            //Arsenal - Chelsea 2-1 (2017-03-06)
            resultList.Add(new Game
            {
                GameId = gameId++,
                HomeTeamId = Teams["Arsenal FC"],
                AwayTeamId = Teams["Chelsea"],
                Date = new DateTime(2017, 3, dayId++),
                HomeTeamGoals = 2,
                AwayTeamGoals = 1,
                Result = DbGameResult.HomeWin
            });

            //Arsenal - Real Madrid 3-2 (2017-03-07)
            resultList.Add(new Game
            {
                GameId = gameId++,
                HomeTeamId = Teams["Arsenal FC"],
                AwayTeamId = Teams["Real Madrid"],
                Date = new DateTime(2017, 3, dayId++),
                HomeTeamGoals = 3,
                AwayTeamGoals = 2,
                Result = DbGameResult.HomeWin
            });

            //Spartak M - Real Madrid 2-2 (2017-03-08)
            resultList.Add(new Game
            {
                GameId = gameId++,
                HomeTeamId = Teams["Spartak M"],
                AwayTeamId = Teams["Real Madrid"],
                Date = new DateTime(2017, 3, dayId++),
                HomeTeamGoals = 2,
                AwayTeamGoals = 2,
                Result = DbGameResult.Draw
            });

            return resultList;
        }

        private SearchEngine searchEngine;

        [SetUp]
        public void Setup()
        {
            //arrange
            var repo = Substitute.For<ILeagueDataRepository>();
            repo
                .GetAllGamesInTimeRange(Arg.Any<DateTime>(), Arg.Any<DateTime>())
                .Returns(ReturnGames());
            searchEngine = new SearchEngine(repo);            
        }

        [Test]
        public void GetGames_ByTeamId_HomeGames_BTSG()
        {
            //arange 
            var conditions = new SearchConditions
            {
                TimeRange = new SearchTimeRange
                {
                    FromDate = new DateTime(2017, 1, 1),
                    ToDate = new DateTime(2017, 1, 2)
                },
                TeamId = Teams["Arsenal FC"],
                BothTeamsScore = true,
                GamePlace = GamePlace.Home
            };

            //act
            var results = searchEngine.GetGames(conditions);

            //assert
            Assert.True(results.Any(g => g.GameId == 1));
            Assert.True(results.Any(g => g.GameId == 6));
            Assert.True(results.Any(g => g.GameId == 7));
            Assert.AreEqual(results.Count, 3);
        }

        [Test]
        public void GetGames_ByTeamId_AllGames_TeamTotal()
        {
            //arange 
            var conditions = new SearchConditions
            {
                TimeRange = new SearchTimeRange
                {
                    FromDate = new DateTime(2017, 1, 1),
                    ToDate = new DateTime(2017, 1, 2)
                },
                TeamId = Teams["Arsenal FC"],
                BothTeamsScore = null,
                TeamTotal = new TeamTotal
                {
                    Team = Team.Team1,
                    GoalsNumber = 1,
                    TotalType = TotalType.Over
                },
                GamePlace = GamePlace.NotDefined
            };

            //act
            var results = searchEngine.GetGames(conditions);

            //assert
            Assert.True(results.Any(g => g.GameId == 1));
            Assert.True(results.Any(g => g.GameId == 6));
            Assert.True(results.Any(g => g.GameId == 7));
            Assert.AreEqual(results.Count, 3);
        }

        [Test]
        public void GetGames_WithoutTeamId_AllGames_BTSG()
        {
            //arange 
            var conditions = new SearchConditions
            {
                TimeRange = new SearchTimeRange
                {
                    FromDate = new DateTime(2017, 1, 1),
                    ToDate = new DateTime(2017, 1, 2)
                },
                TeamId = null,
                BothTeamsScore = true
            };

            //act
            var results = searchEngine.GetGames(conditions);

            //assert
            Assert.True(results.Any(g => g.GameId == 1));
            Assert.True(results.Any(g => g.GameId == 3));
            Assert.True(results.Any(g => g.GameId == 4));
            Assert.True(results.Any(g => g.GameId == 6));
            Assert.True(results.Any(g => g.GameId == 7));
            Assert.True(results.Any(g => g.GameId == 8));
            Assert.AreEqual(results.Count, 6);
        }

        [Test]
        public void GetGames_WithoutTeamId_AllGames_TotalEqual()
        {
            //arange 
            var conditions = new SearchConditions
            {
                TimeRange = new SearchTimeRange
                {
                    FromDate = new DateTime(2017, 1, 1),
                    ToDate = new DateTime(2017, 1, 2)
                },
                TeamId = null,
                BothTeamsScore = null,
                GameTotal = new Total
                {
                    GoalsNumber = 3,
                    TotalType = TotalType.Equal
                }
                
            };

            //act
            var results = searchEngine.GetGames(conditions);

            //assert
            Assert.True(results.Any(g => g.GameId == 1));
            Assert.True(results.Any(g => g.GameId == 6));
            Assert.AreEqual(results.Count, 2);
        }

        [Test]
        public void GetMaximalStreak_WithoutTeamId_AllGames_TotalOver()
        {
            //arange 
            var conditions = new SearchConditions
            {
                TimeRange = new SearchTimeRange
                {
                    FromDate = new DateTime(2017, 1, 1),
                    ToDate = new DateTime(2017, 1, 2)
                },
                TeamId = null,
                BothTeamsScore = null,
                GameTotal = new Total
                {
                    GoalsNumber = 2,
                    TotalType = TotalType.Over
                }

            };

            //act
            var results = searchEngine.GetMaximalStreak(conditions);

            //assert
            Assert.True(results.Any(g => g.GameId == 6));
            Assert.True(results.Any(g => g.GameId == 7));
            Assert.True(results.Any(g => g.GameId == 8));
            Assert.AreEqual(results.Count, 3);
        }

        [Test]
        public void GetMinimalStreak_WithoutTeamId_AllGames_TotalUnder()
        {
            //arange 
            var conditions = new SearchConditions
            {
                TimeRange = new SearchTimeRange
                {
                    FromDate = new DateTime(2017, 1, 1),
                    ToDate = new DateTime(2017, 1, 2)
                },
                TeamId = null,
                BothTeamsScore = null,
                GameTotal = new Total
                {
                    GoalsNumber = 4,
                    TotalType = TotalType.Under
                }

            };

            //act
            var results = searchEngine.GetMinimalStreak(conditions);

            //assert
            Assert.True(results.Any(g => g.GameId == 5));
            Assert.True(results.Any(g => g.GameId == 6));
            Assert.AreEqual(results.Count, 2);
        }

        [Test]
        public void GetNumberOfStreaks_WithoutTeamId_AllGames_BTSG()
        {
            //arange 
            var conditions = new SearchConditions
            {
                TimeRange = new SearchTimeRange
                {
                    FromDate = new DateTime(2017, 1, 1),
                    ToDate = new DateTime(2017, 1, 2)
                },
                TeamId = null,
                BothTeamsScore = true
            };

            var streakCondition = new StreakConditions
            {
                NumberOfItems = 1,
                TotalType = TotalType.Over
            };

            //act
            var results = searchEngine.GetNumberOfStreaks(conditions, streakCondition);

            //assert
            Assert.AreEqual(2, results);
        }

    }
}

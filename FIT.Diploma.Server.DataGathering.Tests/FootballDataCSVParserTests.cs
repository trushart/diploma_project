using FIT.Diploma.Server.DataGathering.Helpers;
using FIT.Diploma.TestingFramework;
using NUnit.Framework;
using System.Linq;

namespace FIT.Diploma.Server.DataGathering.Tests
{
    [TestFixture]
    public class FootballDataCSVParserTests
    {
        private static string TestDataFile = "test.csv";
        [Test]
        public void VerifyParseCsvMethod()
        {
            //arrange
            FootballDataCSVParser parser = new FootballDataCSVParser();
            
            var fileContent = FileLoader.GetFiles(TestDataFile);
            if (fileContent.Length != 1)
                Assert.IsTrue(true, $"Found multiple test data files with name '{TestDataFile}'");

            //act
            var result = parser.ParseCsv(fileContent[0], "-");
            var testGame = result[3];

            //assert
            Assert.AreEqual(10, result.Count);

            //asert 1st game
            Assert.IsNotNull(testGame);
            Assert.AreEqual("Girona", testGame.HomeTeam);
            Assert.AreEqual("Ath Madrid", testGame.AwayTeam);

            Assert.AreEqual(2, testGame.FullTimeHomeTeamGoals);
            Assert.AreEqual(2, testGame.FullTimeAwayTeamGoals);
            Assert.AreEqual("D", testGame.FullTimeResult);

            Assert.AreEqual(2, testGame.HalfTimeHomeTeamGoals);
            Assert.AreEqual(1, testGame.HalfTimeAwayTeamGoals);
            Assert.AreEqual("H", testGame.HalfTimeResult);

            Assert.AreEqual(13, testGame.HomeTeamShots);
            Assert.AreEqual(9, testGame.AwayTeamShots);

            Assert.AreEqual(6, testGame.HomeTeamShotsOnTarget);
            Assert.AreEqual(3, testGame.AwayTeamShotsOnTarget);

            Assert.AreEqual(15, testGame.HomeTeamFouls);
            Assert.AreEqual(15, testGame.AwayTeamFouls);

            Assert.AreEqual(6, testGame.HomeTeamCorners);
            Assert.AreEqual(1, testGame.AwayTeamCorners);

            Assert.AreEqual(2, testGame.HomeTeamYellowCards);
            Assert.AreEqual(4, testGame.AwayTeamYellowCards);
            Assert.AreEqual(0, testGame.HomeTeamRedCards);
            Assert.AreEqual(1, testGame.AwayTeamRedCards);

            //bets
            Assert.AreEqual(8, testGame.B365_HomeWin);
            Assert.AreEqual(4.33, testGame.B365_Draw);
            Assert.AreEqual(1.45, testGame.B365_AwayWin);

            Assert.AreEqual(1.74, testGame.Average_TotalLess2_5);
            Assert.AreEqual(2.11, testGame.Average_TotalMore2_5);
        }
    }
}

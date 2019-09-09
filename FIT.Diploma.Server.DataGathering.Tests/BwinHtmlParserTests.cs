using FIT.Diploma.Server.DataGathering.Helpers;
using FIT.Diploma.TestingFramework;
using NUnit.Framework;
using System;

namespace FIT.Diploma.Server.DataGathering.Tests
{
    [TestFixture]
    public class BwinHtmlParserTests
    {
        [Test]
        public void VerifyGetMatchDetailsUrlMethod()
        {
            //arrange
            BwinHtmlParser parser = new BwinHtmlParser();
            var fileContent = FileLoader.GetFileContent("BwinHtmlExample.txt");

            //act
            var result = parser.GetMatchDetailsUrl(fileContent);

            //assert
            Assert.AreEqual("/en/sports/events/4/16108/6745806/betting/getafe-malaga", result);
        }

        [Test]
        public void VerifyParseOddsMethod()
        {
            //arrange
            BwinHtmlParser parser = new BwinHtmlParser();
            var fileContent = FileLoader.GetFileContent("BwinGameOdds.txt");
            
            //act
            var result = parser.ParseOdds(fileContent);

            //assert
            Assert.AreEqual("Getafe", result.HomeTeam);
            Assert.AreEqual("Malaga", result.AwayTeam);

            Assert.AreEqual(1.62, result.BothTeamsToScore_No);
            Assert.AreEqual(2.15, result.BothTeamsToScore_Yes);

            Assert.AreEqual(2.25, result.Total2_5Over);
            Assert.AreEqual(1.6, result.Total2_5Under);

            Assert.AreEqual(1.22, result.DoubleChanceCoef_12);
            Assert.AreEqual(1.16, result.DoubleChanceCoef_1X);
            Assert.AreEqual(2.1, result.DoubleChanceCoef_X2);

            Assert.AreEqual(1.7, result.HomeWinCoef);
            Assert.AreEqual(3.5, result.DrawCoef);
            Assert.AreEqual(5, result.AwayWinCoef);

            Assert.AreEqual(2.3, result.HTHomeWinCoef);
            Assert.AreEqual(2.05, result.HTDrawCoef);
            Assert.AreEqual(5.25, result.HTAwayWinCoef);
        }
    }
}

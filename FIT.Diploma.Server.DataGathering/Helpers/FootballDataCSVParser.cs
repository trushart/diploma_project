using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.DataGathering.Models;
using LumenWorks.Framework.IO.Csv;
using FIT.Diploma.Server.Database.LeagueData;

namespace FIT.Diploma.Server.DataGathering.Helpers
{
    public class FootballDataCSVParser
    {
        private int _startYear, _endYear;

        private LogRepository logRepo;
        private LeagueDataRepository leagueDataRepo;
        private BookmakerOddsDataRepository bookmakerRepo;

        public FootballDataCSVParser(LogRepository _logRepo = null, LeagueDataRepository _leagueDataRepo = null, BookmakerOddsDataRepository _bookmakerRepo = null)
        {
            logRepo = _logRepo ?? new LogRepository();
            leagueDataRepo = _leagueDataRepo ?? new LeagueDataRepository();
            bookmakerRepo = _bookmakerRepo ?? new BookmakerOddsDataRepository();
        }
        //one CSV file contains all matches of one particular season
        public void ParseCsvAndSaveToDB(string fileName, string season, out int processedGames)
        {
            processedGames = 0;
            if (GetSeasonStartYear(season) == -1)
                throw new Exception("Bad season format");

            var parsedGames = ParseCsv(fileName, season);

            foreach (var game in parsedGames)
                SaveFootballDataModelToDb(game, ref processedGames);
        }

        public List<FootballDataModel> ParseCsv(string fileName, string season)
        {
            var results = new List<FootballDataModel>();

            using (CsvReader csv =
                new CsvReader(new StreamReader(fileName), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();

                while (csv.ReadNextRecord())
                {
                    try
                    {
                        var match = new FootballDataModel();
                        for (int i = 0; i < fieldCount; i++)
                        {
                            string csvValue = string.Empty;
                            try
                            {
                                csvValue = csv[i];
                            }
                            catch (Exception ex)
                            {
                                //file doesn't contain information of other fields
                                break;
                            }

                            if (!match.SetProperty(headers[i], csvValue))
                            {
                                if (!string.IsNullOrEmpty(csvValue))
                                {
                                    string obj = string.Format("[header '{0}'] = [value '{1}'];", headers[i], csvValue);
                                    Console.Write(string.Format("Couldn't set property (possible error). {0}", obj));

                                    logRepo.WriteLog(Database.SystemData.Severity.Warning, "Couldn't parse csv " + obj, nameof(FootballDataCSVParser), "localhost", "", "");
                                }
                            }
                        }
                        results.Add(match);
                    }
                    catch (Exception ex)
                    {
                        logRepo.WriteExceptionLog(ex, nameof(FootballDataCSVParser));
                    }
                }
            }

            return results;
        }

        private void SaveFootballDataModelToDb(FootballDataModel match, ref int processedGames)
        {
            bool onlyCreated;
            var leagueSeason = leagueDataRepo.CreateAndSaveLeagueSeason(_startYear, leagueDataRepo.GetLaLiga());            
            var homeTeam = leagueDataRepo.GetFootballTeam(match.HomeTeam, leagueSeason, "FootballData");
            var awayTeam = leagueDataRepo.GetFootballTeam(match.AwayTeam, leagueSeason, "FootballData");
            Game matchDb = null;

            try
            {
                matchDb = leagueDataRepo.GetGame(homeTeam, awayTeam, match.MatchDate, leagueSeason, out onlyCreated);
            }
            catch(Exception ex)
            {
                return;
            }

            if(onlyCreated || matchDb.Result == 0) //update game record
            {
                //Console.WriteLine("Game record only created - will be processed");
                matchDb.AwayTeamGoals = match.FullTimeAwayTeamGoals;
                matchDb.HomeTeamGoals = match.FullTimeHomeTeamGoals;
                if (matchDb.HomeTeamGoals > matchDb.AwayTeamGoals)
                    matchDb.Result = GameResult.HomeWin;
                else if (matchDb.HomeTeamGoals < matchDb.AwayTeamGoals)
                    matchDb.Result = GameResult.AwayWin;
                else
                    matchDb.Result = GameResult.Draw;

                leagueDataRepo.UpdateGameRecord(matchDb);
                processedGames++;
            }

            var matchStats = leagueDataRepo.GetGameStats(matchDb.GameId, out onlyCreated);
            if (onlyCreated) //update game stats
            {
                matchStats.Game = matchDb;
                matchStats.HalfTimeHomeGoals = match.HalfTimeHomeTeamGoals;
                matchStats.HalfTimeAwayGoals = match.HalfTimeAwayTeamGoals;
                if (matchStats.HalfTimeHomeGoals > matchStats.HalfTimeAwayGoals)
                    matchStats.HalfTimeResult = GameResult.HomeWin;
                else if (matchStats.HalfTimeHomeGoals < matchStats.HalfTimeAwayGoals)
                    matchStats.HalfTimeResult = GameResult.AwayWin;
                else
                    matchStats.HalfTimeResult = GameResult.Draw;

                matchStats.HomeTeamShots = match.HomeTeamShots;
                matchStats.AwayTeamShots = match.AwayTeamShots;

                matchStats.HomeTeamTargetShots = match.HomeTeamShotsOnTarget;
                matchStats.AwayTeamTargetShots = match.AwayTeamShotsOnTarget;

                matchStats.HomeTeamFouls = match.HomeTeamFouls;
                matchStats.AwayTeamFouls = match.AwayTeamFouls;

                matchStats.HomeTeamCorners = match.HomeTeamCorners;
                matchStats.AwayTeamCorners = match.AwayTeamCorners;

                matchStats.HomeTeamYCards = match.HomeTeamYellowCards;
                matchStats.AwayTeamYCards = match.AwayTeamYellowCards;
                matchStats.HomeTeamRedCards = match.HomeTeamRedCards;
                matchStats.AwayTeamRedCards = match.AwayTeamRedCards;

                matchStats.HomeTeamOffsides = match.HomeTeamOffsides;
                matchStats.AwayTeamOffsides = match.AwayTeamOffsides;

                leagueDataRepo.UpdateGameStats(matchStats);
            }

            var bookmaker = bookmakerRepo.GetBookmaker("Bet365");
            var bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.B365_HomeWin;
                bookmakerOdd.DrawCoef = match.B365_Draw;
                bookmakerOdd.AwayWinCoef = match.B365_AwayWin;

                bookmakerOdd.Total2_5Over = match.B365_TotalMore2_5;
                bookmakerOdd.Total2_5Under = match.B365_TotalLess2_5;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("Blue Square");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.BS_HomeWin;
                bookmakerOdd.DrawCoef = match.BS_Draw;
                bookmakerOdd.AwayWinCoef = match.BS_AwayWin;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("Bet&Win");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.BW_HomeWin;
                bookmakerOdd.DrawCoef = match.BW_Draw;
                bookmakerOdd.AwayWinCoef = match.BW_AwayWin;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }            

            bookmaker = bookmakerRepo.GetBookmaker("Gamebookers");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.GB_HomeWin;
                bookmakerOdd.DrawCoef = match.GB_Draw;
                bookmakerOdd.AwayWinCoef = match.GB_AwayWin;

                bookmakerOdd.Total2_5Over = match.GB_TotalMore2_5;
                bookmakerOdd.Total2_5Under = match.GB_TotalLess2_5;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("Interwetten");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.IW_HomeWin;
                bookmakerOdd.DrawCoef = match.IW_Draw;
                bookmakerOdd.AwayWinCoef = match.IW_AwayWin;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("Pinnacle");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.Pinnacle_HomeWin;
                bookmakerOdd.DrawCoef = match.Pinnacle_Draw;
                bookmakerOdd.AwayWinCoef = match.Pinnacle_AwayWin;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("Sporting Odds");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.SO_HomeWin;
                bookmakerOdd.DrawCoef = match.SO_Draw;
                bookmakerOdd.AwayWinCoef = match.SO_AwayWin;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("Sportingbet");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.SB_HomeWin;
                bookmakerOdd.DrawCoef = match.SB_Draw;
                bookmakerOdd.AwayWinCoef = match.SB_AwayWin;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("Stan James");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.SJ_HomeWin;
                bookmakerOdd.DrawCoef = match.SJ_Draw;
                bookmakerOdd.AwayWinCoef = match.SJ_AwayWin;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("Stanleybet");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.Stanleybet_HomeWin;
                bookmakerOdd.DrawCoef = match.Stanleybet_Draw;
                bookmakerOdd.AwayWinCoef = match.Stanleybet_AwayWin;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("VC");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.VC_HomeWin;
                bookmakerOdd.DrawCoef = match.VC_Draw;
                bookmakerOdd.AwayWinCoef = match.VC_AwayWin;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("William Hill");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.HomeWinCoef = match.WH_HomeWin;
                bookmakerOdd.DrawCoef = match.WH_Draw;
                bookmakerOdd.AwayWinCoef = match.WH_AwayWin;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("Total average odds");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.Total2_5Over = match.Average_TotalMore2_5;
                bookmakerOdd.Total2_5Under = match.Average_TotalLess2_5;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }

            bookmaker = bookmakerRepo.GetBookmaker("Total maximal odds");
            bookmakerOdd = bookmakerRepo.GetBookmakerOdds(matchDb.GameId, bookmaker.BookmakerId, out onlyCreated);
            if (onlyCreated) //update game record
            {
                bookmakerOdd.Total2_5Over = match.Max_TotalMore2_5;
                bookmakerOdd.Total2_5Under = match.Max_TotalLess2_5;

                bookmakerRepo.UpdateBookmakerOdds(bookmakerOdd);
            }
        }

        //return year or -1 if not correct format (format shoul be 4 numbers first two numbers = start year, next two = end year. F.e. 1718 = 2017-2018)
        private int GetSeasonStartYear(string season)
        {
            if (season.Length != 4 || !IsDigitsOnly(season)) return -1;
            _startYear = Int32.Parse(season.Substring(0, 2));
            _endYear = Int32.Parse(season.Substring(2, 2));
            if (_endYear - _startYear != 1 && _endYear - _startYear != -99) return -1;
            _startYear = NormilizeYear(_startYear);
            _endYear = NormilizeYear(_endYear);
            return _startYear;
        }

        private int NormilizeYear(int year)
        {
            if (year <= 20) return 2000 + year;
            else return 1900 + year;
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}

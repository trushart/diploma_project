using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIT.Diploma.Server.DataAnalysis
{
    public class RoundsStatsAnalyzer : IDataAnalyzer
    {
        private ILeagueDataRepository leagueRepo = new LeagueDataRepository();
        private SystemDataRepository systemRepo = new SystemDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();
        private BookmakerOddsDataRepository bookRepo = new BookmakerOddsDataRepository();

        public string GetTargetDataTable()
        {
            return "SeasonRound";
        }

        public void RunAnalyzing()
        {
            var allSeasons = leagueRepo.GetAllLeagueSeasons(leagueRepo.GetLaLiga());
            foreach (var season in allSeasons)
            {
                //if (season.LeagueSeasonId != 12 && season.LeagueSeasonId != 13 && season.LeagueSeasonId != 14)
                //    continue;
                Console.WriteLine($"Start analyzing league season: {season.LeagueSeasonId} [{season.StartYear}-{season.StartYear + 1}] .");
                AnalyzeLeagueSeason(season);
                Console.WriteLine($"Finish analyzing");
            }
        }

        private void AnalyzeLeagueSeason(LeagueSeason season)
        {
            var seasonRounds = leagueRepo.GetCurrentSeasonRounds(season.LeagueSeasonId).OrderBy(r => r.RoundId).ToList();
            var check = CheckSeasonGames(season.LeagueSeasonId);            
            check &= CheckAllRounds(seasonRounds);
            
            if (!check)
            {
                //rounds may be updated
                CheckRoundGames(season.LeagueSeasonId, seasonRounds);
                CheckAllRounds(seasonRounds);
                seasonRounds = leagueRepo.GetCurrentSeasonRounds(season.LeagueSeasonId);
                DeleteGarbageRounds(seasonRounds);
                seasonRounds = leagueRepo.GetCurrentSeasonRounds(season.LeagueSeasonId);
            }

            var analyzedGames = systemRepo.GetSeasonRoundAnalyzedGames(season.LeagueSeasonId);
            var processedGames = new List<int>();

            foreach (var round in seasonRounds)
            {
                var roundGames = leagueRepo.GetAllRoundGames(round.RoundId);
                var gamePlayed = 0;

                foreach(var game in roundGames)
                {
                    if (analyzedGames.FindIndex(g => g.GameId == game.GameId) >= 0)
                    {
                        gamePlayed++;
                        continue;
                    }
                    if (game.Result == 0) continue;
                    var gameTotal = game.HomeTeamGoals + game.AwayTeamGoals;
                    

                    if (game.Result == GameResult.HomeWin)
                        round.HomeWinsCount++;
                    else if (game.Result == GameResult.AwayWin)
                        round.AwayWinsCount++;
                    else if (game.Result == GameResult.Draw)
                        round.DrawsCount++;

                    round.Goals += gameTotal;
                    round.HomeGoals += game.HomeTeamGoals;
                    round.AwayGoals += game.AwayTeamGoals;

                    if (gameTotal > 2.5)
                        round.GamesOver2_5++;
                    else
                        round.GamesUnder2_5++;

                    if (game.HomeTeamGoals > 0 && game.AwayTeamGoals > 0)
                        round.BTTS_Yes++;
                    else
                        round.BTTS_No++;

                    gamePlayed++;
                    processedGames.Add(game.GameId);
                }
                if(round.GamePlayed != gamePlayed)
                {
                    Console.WriteLine($"!!! Round played games number is incorrect !!! [roundID = {round.RoundId}][round.GamePlayed = {round.GamePlayed}][gamePlayed = {gamePlayed}]");
                }
                leagueRepo.UpdateSeasonRound(round);
            }

            systemRepo.AddAnalyzedGames_SeasonRounds(processedGames, season.LeagueSeasonId);
        }

        private void DeleteGarbageRounds(List<SeasonRound> rounds)
        {
            var garbageRoundIds = rounds.Where(r => r.GamePlayed == 0).Select(s => s.RoundId).ToList();
            foreach(var garbageRoundId in garbageRoundIds)
            {
                leagueRepo.RemoveSeasonRound(garbageRoundId);
            }
        }

        private bool CheckAllRounds(List<SeasonRound> rounds)
        {
            var result = true;
            var roundNumbers = 1;

            foreach(var round in rounds)
            {
                var roundGames = leagueRepo.GetAllRoundGames(round.RoundId);
                if (roundGames.Count != round.GamePlayed || round.GamePlayed == 0)
                    result = false;
                
                if((round.HomeWinsCount + round.DrawsCount + round.AwayWinsCount) > round.GamePlayed)
                {
                    round.HomeWinsCount = 0;
                    round.DrawsCount = 0;
                    round.AwayWinsCount = 0;
                    round.AwayGoals = 0;
                    round.HomeGoals = 0;
                }
                if ((round.AwayGoals + round.HomeGoals) != round.Goals)
                {
                    round.AwayGoals = 0;
                    round.HomeGoals = 0;
                    round.Goals = 0;
                }
                if ((round.GamesOver2_5 + round.GamesUnder2_5) > round.GamePlayed)
                {
                    round.GamesOver2_5 = 0;
                    round.GamesUnder2_5 = 0;
                }
                if ((round.BTTS_Yes + round.BTTS_No) > round.GamePlayed)
                {
                    round.BTTS_Yes = 0;
                    round.BTTS_No = 0;
                }

                round.GamePlayed = roundGames.Count;
                round.IsFinished = (roundGames.Count >= 10) && (round.GamePlayed == round.BTTS_No + round.BTTS_Yes)
                                    && (round.GamePlayed == round.GamesOver2_5 + round.GamesUnder2_5)
                                    && (round.GamePlayed == round.HomeWinsCount + round.DrawsCount + round.AwayWinsCount);
                round.RoundNumber = roundNumbers++;
                leagueRepo.UpdateSeasonRound(round);
            }

            return result;
        }

        private bool CheckRoundGames(int seasonId, List<SeasonRound> rounds)
        {
            var result = true;            
            var allGames = leagueRepo.GetAllSeasonGames(seasonId).OrderBy(g => g.Date).ToList();
            var nextRoundIndex = 1;
            var currentRound = rounds.First();
            var i = 0;

            foreach (var game in allGames)
            {
                if(i == 10)
                {
                    i = 0;
                    currentRound = rounds[nextRoundIndex++];
                }
                game.RoundId = currentRound.RoundId;
                leagueRepo.UpdateGameRecord(game);
                i++;
            }

            return result; ;
        }

        private bool CheckSeasonGames(int seasonId)
        {
            var result = true;
            var allGames = leagueRepo.GetAllSeasonGames(seasonId);
            var repeatedGames = allGames.GroupBy(g => new { g.HomeTeamId, g.AwayTeamId }).Where(gr => gr.Count() > 1);
            foreach(var group in repeatedGames)
            {
                if (group.Count() > 2)
                    Console.WriteLine($"One match has more then 2 records in DB!!! HomeTeamId = {group.Key.HomeTeamId} AwayTeamId = {group.Key.AwayTeamId}");
                bool ignoreThrowing = false;
                var gamesWithoutResult = group.Where(g => g.Result == 0).ToList();
                try
                {
                    ThrowIfNotRightCount(gamesWithoutResult, 1);
                }
                catch(Exception ex)
                {
                    if (ex.Message.Contains("Too many")) ignoreThrowing = true;
                }
                var gameWithResult = group.Where(g => g.Result != 0).ToList();
                ThrowIfNotRightCount(gameWithResult, 1, ignoreThrowing);

                Game toReplace, toRemove;
                if (gamesWithoutResult.Count > 1)
                {
                    for(int i = 1; i < gamesWithoutResult.Count; i++)
                    {
                        toReplace = gamesWithoutResult.First();
                        toRemove = gamesWithoutResult[i];
                        RemoveGame(toReplace, toRemove);
                    }
                }

                if(gameWithResult.Any())
                {
                    toReplace = gameWithResult.First();
                    toRemove = gamesWithoutResult.First();
                    RemoveGame(toReplace, toRemove);
                }                
                
            }
            return result;
        }

        private void RemoveGame(Game toReplace, Game toRemove)
        {
            
            analysisRepo.UpdateGameIdForPredictions(toRemove.GameId, toReplace.GameId);
            bookRepo.UpdateGameIdForBookmakerOdds(toRemove.GameId, toReplace.GameId);

            systemRepo.DeleteRecord_BookmakerOddsStats_AnalyzedGames(toRemove.GameId);
            systemRepo.DeleteRecord_HeadToHeadStats_AnalyzedGames(toRemove.GameId);
            systemRepo.DeleteRecord_SeasonRound_AnalyzedGames(toRemove.GameId);
            systemRepo.DeleteRecord_SeasonStats_AnalyzedGames(toRemove.GameId);
            systemRepo.DeleteRecord_StandingTable_AnalyzedGames(toRemove.GameId);

            systemRepo.DeleteRecord_SeasonRound_AnalyzedGames(toReplace.GameId);

            leagueRepo.DeleteGameStats(toRemove.GameId);
            leagueRepo.DeleteGameRecord(toRemove.GameId);
        }

        private void ThrowIfNotRightCount(List<Game> list, int count, bool ignoreThrowing = false)
        {
            if(list.Count > count)
            {
                Console.WriteLine($"There are too many games in the list {list.Count}. Right count = {count}");
                throw new Exception("Too many items in List<Games>");
            }
            if (list.Count < count)
            {
                Console.WriteLine($"There are not enough games in the list {list.Count}. Right count = {count}");
                if (!ignoreThrowing) throw new Exception("Not enough items in List<Games>");
            }
        }
    }
}

using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.AnalyzerResults;
using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;

namespace FIT.Diploma.Server.DataAnalysis
{
    public class HeadToHeadStatsAnalyzer : IDataAnalyzer
    {
        private ILeagueDataRepository leagueRepo = new LeagueDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();
        private SystemDataRepository systemRepo = new SystemDataRepository();

        public string GetTargetDataTable()
        {
            return "HeadToHeadStats";
        }

        public void RunAnalyzing()
        {
            //1. step = get all upcoming games
            var upcomingGames = leagueRepo.GetAllUpcomingGames();

            //2. step = for all upcoming games analyzed H2H
            foreach(var game in upcomingGames)
            {
                AnalyzeGameCompetitoresH2H(game);
            }
        }

        private void AnalyzeGameCompetitoresH2H(Game game)
        {
            var analyzedGames = systemRepo.GetHeadToHeadStatsAnalyzedGames();
            if (game.Result != 0 || analyzedGames.FindIndex(g => g.GameId == game.GameId) >= 0)
            {
                Console.WriteLine($"Game '{game.GameId}' already have been analyzed.");
                return;
            }

            var teamsHeadToHead = analysisRepo.GetTeamsHeadToHead(game.HomeTeamId, game.AwayTeamId);
            var updatedHeadToHead = new HeadToHeadStats
            {
                Team1Id = teamsHeadToHead.Team1Id,
                Team2Id = teamsHeadToHead.Team2Id
            };

            var teamsHistory = leagueRepo.GetTeamsHistory(teamsHeadToHead.Team1Id, teamsHeadToHead.Team2Id);

            foreach(var historyGame in teamsHistory)
            {
                if (!ComputeH2hForGame(updatedHeadToHead, historyGame)) return;
            }

            analysisRepo.UpdateHeadToHeadStats(updatedHeadToHead);
            systemRepo.AddAnalyzedGames_HeadToHeadStats(new List<int> { game.GameId}, game.SeasonRound.LeagueSeasonId);
        }

        private bool ComputeH2hForGame(HeadToHeadStats stats, Game game)
        {
            if(game.HomeTeamId == stats.Team1Id && game.AwayTeamId == stats.Team2Id)
            {
                //game results
                if (game.Result == GameResult.HomeWin)
                    stats.Team1WinsCount++;
                else if (game.Result == GameResult.AwayWin)
                    stats.Team2WinsCount++;
                else stats.DrawsCount++;

                //teamGoals
                stats.Team1Goals += game.HomeTeamGoals;
                stats.Team2Goals += game.AwayTeamGoals;
            }
            else if (game.HomeTeamId == stats.Team2Id && game.AwayTeamId == stats.Team1Id)
            {
                //game results
                if (game.Result == GameResult.HomeWin)
                    stats.Team2WinsCount++;
                else if (game.Result == GameResult.AwayWin)
                    stats.Team1WinsCount++;
                else stats.DrawsCount++;

                //teamGoals
                stats.Team1Goals += game.AwayTeamGoals;
                stats.Team2Goals += game.HomeTeamGoals;
            }
            else
            {
                Console.WriteLine($"Error accuared while processing game [gameId={game.GameId}]. teamIDs = [{game.HomeTeamId},{game.AwayTeamId}] [{stats.Team1Id},{stats.Team2Id}]");
                return false;
            }
                

            //total
            var total = game.AwayTeamGoals + game.HomeTeamGoals;
            stats.Goals += total;
            if (total > 2.5)
                stats.GamesOver2_5++;
            else
                stats.GamesUnder2_5++;

            //btts
            if (game.AwayTeamGoals > 0 && game.HomeTeamGoals > 0)
                stats.BTTS_Yes++;
            else
                stats.BTTS_No++;

            stats.GamePlayed++;
            //per game
            stats.GoalsPerGame = (double)stats.Goals / stats.GamePlayed;
            stats.Team1GoalsPerGame = (double)stats.Team1Goals / stats.GamePlayed;
            stats.Team2GoalsPerGame = (double)stats.Team2Goals / stats.GamePlayed;

            stats.Team1WinsPercentage = (double)stats.Team1WinsCount / stats.GamePlayed;
            stats.Team2WinsPercentage = (double)stats.Team2WinsCount / stats.GamePlayed;
            stats.DrawsPercentage = (double)stats.DrawsCount / stats.GamePlayed;
            return true;
        }
    }
}

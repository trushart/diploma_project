using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.AnalyzerResults;
using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIT.Diploma.Server.DataAnalysis
{
    public class SeasonStatsAnalyzer : IDataAnalyzer
    {
        private ILeagueDataRepository leagueRepo = new LeagueDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();
        private SystemDataRepository systemRepo = new SystemDataRepository();

        public string GetTargetDataTable()
        {
            return "SeasonStats";
        }

        public void RunAnalyzing()
        {
            var allSeasons = leagueRepo.GetAllLeagueSeasons(leagueRepo.GetLaLiga());
            foreach (var season in allSeasons)
            {
                var seasonStats = analysisRepo.GetSeasonStats(season.LeagueSeasonId);
                if (seasonStats.GamePlayed < 380)
                    AnalyzeLeagueSeason(seasonStats);
                else
                    Console.WriteLine($"Season: {season.LeagueSeasonId} [{season.StartYear}-{season.StartYear + 1}] have been already analyzed. Skip.");
                UpdateLeagueSeason(season);
            }
        }

        private void UpdateLeagueSeason(LeagueSeason season)
        {
            var seasonTeams = analysisRepo.GetSeasonTeamsCount(season.LeagueSeasonId);
            var seasonRound = leagueRepo.GetSeasonRoundCount(season.LeagueSeasonId);
            var allSeasonGames = leagueRepo.GetAllSeasonGames(season.LeagueSeasonId).OrderBy(g => g.Date);

            season.CountOfRounds = seasonRound;
            season.CountOfTeams = seasonTeams;
            if (allSeasonGames.Any()) {
                season.StartDate = allSeasonGames.First().Date;
                if (allSeasonGames.Count() >= 380)
                    season.EndDate = allSeasonGames.Last().Date;
            }

            leagueRepo.UpdateLeagueSeason(season);
        }

        private void AnalyzeLeagueSeason(SeasonStats stats)
        {
            var allSeasonGames = leagueRepo.GetAllSeasonGames(stats.LeagueSeasonId);
            var analyzedGames = systemRepo.GetSeasonStatsAnalyzedGames(stats.LeagueSeasonId);

            var processedGames = new List<int>();
            foreach (var game in allSeasonGames)
            {
                if (game.Result == 0 || analyzedGames.FindIndex(g => g.GameId == game.GameId) >= 0) continue;
                var gameTotal = game.HomeTeamGoals + game.AwayTeamGoals;

                stats.GamePlayed++;

                if (game.Result == GameResult.HomeWin)
                    stats.HomeWinsCount++;
                else if (game.Result == GameResult.AwayWin)
                    stats.AwayWinsCount++;
                else if (game.Result == GameResult.Draw)
                    stats.DrawsCount++;

                stats.HomeWinsPercentage = (double)stats.HomeWinsCount / stats.GamePlayed;
                stats.AwayWinsPercentage = (double)stats.AwayWinsCount / stats.GamePlayed;
                stats.DrawsPercentage = (double)stats.DrawsCount / stats.GamePlayed;

                stats.Goals += gameTotal;
                stats.HomeTeamsGoals += game.HomeTeamGoals;
                stats.AwayTeamsGoals += game.AwayTeamGoals;
                stats.GoalsPerGame = (double)stats.Goals / stats.GamePlayed;

                if (gameTotal > 2.5)
                    stats.GamesOver2_5++;
                else
                    stats.GamesUnder2_5++;

                if (game.HomeTeamGoals > 0 && game.AwayTeamGoals > 0)
                    stats.BTTS_Yes++;
                else
                    stats.BTTS_No++;

                processedGames.Add(game.GameId);
            }
            
            analysisRepo.UpdateSeasonStats(stats);
            systemRepo.AddAnalyzedGames_SeasonStats(processedGames, stats.LeagueSeasonId);
        }
    }
}

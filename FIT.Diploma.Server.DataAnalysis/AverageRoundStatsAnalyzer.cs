using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.AnalyzerResults;
using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Linq;

namespace FIT.Diploma.Server.DataAnalysis
{
    public class AverageRoundStatsAnalyzer : IDataAnalyzer
    {
        private ILeagueDataRepository leagueRepo = new LeagueDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();
        private SystemDataRepository systemRepo = new SystemDataRepository();

        public string GetTargetDataTable()
        {
            return "AverageRoundStats";
        }

        public void RunAnalyzing()
        {
            var allSeasons = leagueRepo.GetAllLeagueSeasons(leagueRepo.GetLaLiga());
            var analyzedSeasons = systemRepo.GetAverageRoundStatsAnalyzedSeasons();
            
            foreach (var season in allSeasons)
            {
                var analyzedSeasonId = analyzedSeasons.FindIndex(s => s.LeagueSeasonId == season.LeagueSeasonId);
                if (analyzedSeasonId >= 0 && analyzedSeasons[analyzedSeasonId].AnalysisFinished)
                {
                    Console.WriteLine($"Season {season.LeagueSeasonId} already analyzed.");
                    continue;
                }

                var isSeasonFinished = AnalyzeLeagueSeason(season);

                if (season.EndDate < DateTime.Now && isSeasonFinished)
                    systemRepo.AddAnalyzedSeasons_AverageRoundStats(season.LeagueSeasonId);
            }
        }

        private bool AnalyzeLeagueSeason(LeagueSeason season)
        {
            var haveAllRoundsStats = false;
            var allSeasonRounds = leagueRepo.GetCurrentSeasonRounds(season.LeagueSeasonId).Where(sr => (sr.HomeWinsCount + sr.DrawsCount + sr.AwayWinsCount) == 10).ToList();           

            var averageRoundStats = new AverageRoundStats
            {
                LeagueSeasonId = season.LeagueSeasonId
            };

            var homeTeamWinsCount = allSeasonRounds.Select(o => o.HomeWinsCount).OrderBy(o => o).ToArray();
            var awayTeamWinsCount = allSeasonRounds.Select(o => o.AwayWinsCount).OrderBy(o => o).ToArray();
            var drawsCount = allSeasonRounds.Select(o => o.DrawsCount).OrderBy(o => o).ToArray();

            var goals = allSeasonRounds.Select(o => o.Goals).OrderBy(o => o).ToArray();
            var awayGoals = allSeasonRounds.Select(o => o.AwayGoals).OrderBy(o => o).ToArray();
            var homeGoals = allSeasonRounds.Select(o => o.HomeGoals).OrderBy(o => o).ToArray();

            var btts_yes = allSeasonRounds.Select(o => o.BTTS_Yes).OrderBy(o => o).ToArray();
            var btts_no = allSeasonRounds.Select(o => o.BTTS_No).OrderBy(o => o).ToArray();

            var totalOver2_5 = allSeasonRounds.Select(o => o.GamesOver2_5).OrderBy(o => o).ToArray();
            var totalUnder2_5 = allSeasonRounds.Select(o => o.GamesUnder2_5).OrderBy(o => o).ToArray();

            averageRoundStats.HomeWinsCount_Average = homeTeamWinsCount.Length != 0 ? homeTeamWinsCount.Average() : 0;
            averageRoundStats.HomeWinsCount_Min = homeTeamWinsCount.Length != 0 ? homeTeamWinsCount.First() : 0;
            averageRoundStats.HomeWinsCount_Max = homeTeamWinsCount.Length != 0 ? homeTeamWinsCount.Last() : 0;
            averageRoundStats.AwayWinsCount_Average = awayTeamWinsCount.Length != 0 ? awayTeamWinsCount.Average() : 0;
            averageRoundStats.AwayWinsCount_Min = awayTeamWinsCount.Length != 0 ? awayTeamWinsCount.First() : 0;
            averageRoundStats.AwayWinsCount_Max = awayTeamWinsCount.Length != 0 ? awayTeamWinsCount.Last() : 0;
            averageRoundStats.DrawsCount_Average = drawsCount.Length != 0 ? drawsCount.Average() : 0;
            averageRoundStats.DrawsCount_Min = drawsCount.Length != 0 ? drawsCount.First() : 0;
            averageRoundStats.DrawsCount_Max = drawsCount.Length != 0 ? drawsCount.Last() : 0;

            averageRoundStats.HomeGoals_Average = homeGoals.Length != 0 ? homeGoals.Average() : 0;
            averageRoundStats.HomeGoals_Min = homeGoals.Length != 0 ? homeGoals.First() : 0;
            averageRoundStats.HomeGoals_Max = homeGoals.Length != 0 ? homeGoals.Last() : 0;
            averageRoundStats.AwayGoals_Average = awayGoals.Length != 0 ? awayGoals.Average() : 0;
            averageRoundStats.AwayGoals_Min = awayGoals.Length != 0 ? awayGoals.First() : 0;
            averageRoundStats.AwayGoals_Max = awayGoals.Length != 0 ? awayGoals.Last() : 0;
            averageRoundStats.Goals_Average = goals.Length != 0 ? goals.Average() : 0;
            averageRoundStats.Goals_Min = goals.Length != 0 ? goals.First() : 0;
            averageRoundStats.Goals_Max = goals.Length != 0 ? goals.Last() : 0;

            averageRoundStats.BTTS_Yes_Average = btts_yes.Length != 0 ? btts_yes.Average() : 0;
            averageRoundStats.BTTS_Yes_Min = btts_yes.Length != 0 ? btts_yes.First() : 0;
            averageRoundStats.BTTS_Yes_Max = btts_yes.Length != 0 ? btts_yes.Last() : 0;
            averageRoundStats.BTTS_No_Average = btts_no.Length != 0 ? btts_no.Average() : 0;
            averageRoundStats.BTTS_No_Min = btts_no.Length != 0 ? btts_no.First() : 0;
            averageRoundStats.BTTS_No_Max = btts_no.Length != 0 ? btts_no.Last() : 0;

            averageRoundStats.GamesOver2_5_Average = totalOver2_5.Length != 0 ? totalOver2_5.Average() : 0;
            averageRoundStats.GamesOver2_5_Min = totalOver2_5.Length != 0 ? totalOver2_5.First() : 0;
            averageRoundStats.GamesOver2_5_Max = totalOver2_5.Length != 0 ? totalOver2_5.Last() : 0;
            averageRoundStats.GamesUnder2_5_Average = totalUnder2_5.Length != 0 ? totalUnder2_5.Average() : 0;
            averageRoundStats.GamesUnder2_5_Min = totalUnder2_5.Length != 0 ? totalUnder2_5.First() : 0;
            averageRoundStats.GamesUnder2_5_Max = totalUnder2_5.Length != 0 ? totalUnder2_5.Last() : 0;

            averageRoundStats.LastUpdate = DateTime.Now;

            //validate stats
            {
                //all main results
                var games = averageRoundStats.HomeWinsCount_Average +
                            averageRoundStats.AwayWinsCount_Average +
                            averageRoundStats.DrawsCount_Average;
                games /= 10;
                if (games < 0.95 || games > 1.01)
                    return ReturnFalse($"[Season = {season.LeagueSeasonId}]All games percentage not 100% [{games * 100}%]");

                var btts = averageRoundStats.BTTS_Yes_Average +
                            averageRoundStats.BTTS_No_Average;
                btts /= 10;
                if (btts < 0.95 || btts > 1.01)
                    return ReturnFalse($"[Season = {season.LeagueSeasonId}]All BTTS games percentage not 100% [{games * 100}%]");

                var total = averageRoundStats.GamesOver2_5_Average +
                            averageRoundStats.GamesUnder2_5_Average;
                total /= 10;
                if (total < 0.95 || total > 1.01)
                    return ReturnFalse($"[Season = {season.LeagueSeasonId}]All TOTAL games percentage not 100% [{games * 100}%]");
            }

            analysisRepo.AddOrUpdateAverageRoundStats(averageRoundStats);

            var gamePlayed = allSeasonRounds.Sum(r => r.GamePlayed);
            haveAllRoundsStats = gamePlayed >= 380;

            return haveAllRoundsStats;
        }

        private bool ReturnFalse(string message)
        {
            Console.WriteLine(message);
            return false;
        }
    }
}

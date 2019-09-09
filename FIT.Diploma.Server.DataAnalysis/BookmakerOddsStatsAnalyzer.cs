using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.AnalyzerResults;
using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIT.Diploma.Server.DataAnalysis
{
    public class BookmakerOddsStatsAnalyzer : IDataAnalyzer
    {
        private ILeagueDataRepository leagueRepo = new LeagueDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();
        private SystemDataRepository systemRepo = new SystemDataRepository();
        private BookmakerOddsDataRepository bookyRepo = new BookmakerOddsDataRepository();

        public string GetTargetDataTable()
        {
            return "FootballTeamForm";
        }

        public void RunAnalyzing()
        {
            var allSeasons = leagueRepo.GetAllLeagueSeasons(leagueRepo.GetLaLiga());
            foreach (var season in allSeasons)
            {
                Console.WriteLine($"Start analyzing league season: {season.LeagueSeasonId} [{season.StartYear}-{season.StartYear + 1}] .");
                AnalyzeLeagueSeason(season);
                Console.WriteLine($"Finish analyzing");
            }
        }

        private void AnalyzeLeagueSeason(LeagueSeason season)
        {
            var allSeasonGames = leagueRepo.GetAllSeasonGames(season.LeagueSeasonId);
            var analyzedGames = systemRepo.GetBookmakerOddsStatsAnalyzeGames(season.LeagueSeasonId);

            var processedGames = new List<int>();
            foreach (var game in allSeasonGames)
            {
                if (game.Result == 0 || analyzedGames.FindIndex(g => g.GameId == game.GameId) >= 0) continue;
                var gameOddsStats = new BookmakerOddsStats
                {
                    GameId = game.GameId
                };

                ComputeBookmakerOddsStats(gameOddsStats, game);

                analysisRepo.AddOrUpdateBookmakerOddsStats(gameOddsStats);
                processedGames.Add(game.GameId);
            }

            systemRepo.AddAnalyzedGames_BookmakerOddsStats(processedGames, season.LeagueSeasonId);
        }

        private void ComputeBookmakerOddsStats(BookmakerOddsStats oddsStats, Game game)
        {
            var allGamesOdds = bookyRepo.GetAllBookmakerOddsForGame(game.GameId);

            var homeTeamWinsCoefs = allGamesOdds.Where(o => o.HomeWinCoef != 0).Select(o => o.HomeWinCoef).OrderBy(o => o).ToArray();
            var awayTeamWinsCoefs = allGamesOdds.Where(o => o.AwayWinCoef != 0).Select(o => o.AwayWinCoef).OrderBy(o => o).ToArray();
            var drawCoefs = allGamesOdds.Where(o => o.DrawCoef != 0).Select(o => o.DrawCoef).OrderBy(o => o).ToArray();

            var doubleChance_1X_coefs = allGamesOdds.Where(o => o.DoubleChanceCoef_1X != 0).Select(o => o.DoubleChanceCoef_1X).OrderBy(o => o).ToArray();
            var doubleChance_X2_coefs = allGamesOdds.Where(o => o.DoubleChanceCoef_X2 != 0).Select(o => o.DoubleChanceCoef_X2).OrderBy(o => o).ToArray();
            var doubleChance_12_coefs = allGamesOdds.Where(o => o.DoubleChanceCoef_12 != 0).Select(o => o.DoubleChanceCoef_12).OrderBy(o => o).ToArray();

            var bothTeamsToScore_yes_coefs = allGamesOdds.Where(o => o.BothTeamsToScore_Yes != 0).Select(o => o.BothTeamsToScore_Yes).OrderBy(o => o).ToArray();
            var bothTeamsToScore_no_coefs = allGamesOdds.Where(o => o.BothTeamsToScore_No != 0).Select(o => o.BothTeamsToScore_No).OrderBy(o => o).ToArray();

            var total2_5_over_coefs = allGamesOdds.Where(o => o.Total2_5Over != 0).Select(o => o.Total2_5Over).OrderBy(o => o).ToArray();
            var total2_5_under_coefs = allGamesOdds.Where(o => o.Total2_5Under != 0).Select(o => o.Total2_5Under).OrderBy(o => o).ToArray();

            var ht_homeTeamWinsCoefs = allGamesOdds.Where(o => o.HTHomeWinCoef != 0).Select(o => o.HTHomeWinCoef).OrderBy(o => o).ToArray();
            var ht_awayTeamWinsCoefs = allGamesOdds.Where(o => o.HTAwayWinCoef != 0).Select(o => o.HTAwayWinCoef).OrderBy(o => o).ToArray();
            var ht_drawCoefs = allGamesOdds.Where(o => o.HTDrawCoef != 0).Select(o => o.HTDrawCoef).OrderBy(o => o).ToArray();

            //main odds
            oddsStats.HomeWinCoef_Average = homeTeamWinsCoefs.Length != 0 ? homeTeamWinsCoefs.Average() : 0;
            oddsStats.HomeWinCoef_Min = homeTeamWinsCoefs.Length != 0 ? homeTeamWinsCoefs.First() : 0;
            oddsStats.HomeWinCoef_Max = homeTeamWinsCoefs.Length != 0 ? homeTeamWinsCoefs.Last() : 0;

            oddsStats.AwayWinCoef_Average = awayTeamWinsCoefs.Length != 0 ? awayTeamWinsCoefs.Average() : 0;
            oddsStats.AwayWinCoef_Min = awayTeamWinsCoefs.Length != 0 ? awayTeamWinsCoefs.First() : 0;
            oddsStats.AwayWinCoef_Max = awayTeamWinsCoefs.Length != 0 ? awayTeamWinsCoefs.Last() : 0;

            oddsStats.DrawCoef_Average = drawCoefs.Length != 0 ? drawCoefs.Average() : 0;
            oddsStats.DrawCoef_Min = drawCoefs.Length != 0 ? drawCoefs.First() : 0;
            oddsStats.DrawCoef_Max = drawCoefs.Length != 0 ? drawCoefs.Last() : 0;

            //double chances
            oddsStats.DoubleChanceCoef_1X_Average = doubleChance_1X_coefs.Length != 0 ? doubleChance_1X_coefs.Average() : 0;
            oddsStats.DoubleChanceCoef_1X_Min = doubleChance_1X_coefs.Length != 0 ? doubleChance_1X_coefs.First() : 0;
            oddsStats.DoubleChanceCoef_1X_Max = doubleChance_1X_coefs.Length != 0 ? doubleChance_1X_coefs.Last() : 0;

            oddsStats.DoubleChanceCoef_X2_Average = doubleChance_X2_coefs.Length != 0 ? doubleChance_X2_coefs.Average() : 0;
            oddsStats.DoubleChanceCoef_X2_Min = doubleChance_X2_coefs.Length != 0 ? doubleChance_X2_coefs.First() : 0;
            oddsStats.DoubleChanceCoef_X2_Max = doubleChance_X2_coefs.Length != 0 ? doubleChance_X2_coefs.Last() : 0;

            oddsStats.DoubleChanceCoef_12_Average = doubleChance_12_coefs.Length != 0 ? doubleChance_12_coefs.Average() : 0;
            oddsStats.DoubleChanceCoef_12_Min = doubleChance_12_coefs.Length != 0 ? doubleChance_12_coefs.First() : 0;
            oddsStats.DoubleChanceCoef_12_Max = doubleChance_12_coefs.Length != 0 ? doubleChance_12_coefs.Last() : 0;

            //btts
            oddsStats.BothTeamsToScore_Yes_Average = bothTeamsToScore_yes_coefs.Length != 0 ? bothTeamsToScore_yes_coefs.Average() : 0;
            oddsStats.BothTeamsToScore_Yes_Min = bothTeamsToScore_yes_coefs.Length != 0 ? bothTeamsToScore_yes_coefs.First() : 0;
            oddsStats.BothTeamsToScore_Yes_Max = bothTeamsToScore_yes_coefs.Length != 0 ? bothTeamsToScore_yes_coefs.Last() : 0;

            oddsStats.BothTeamsToScore_No_Average = bothTeamsToScore_no_coefs.Length != 0 ? bothTeamsToScore_no_coefs.Average() : 0;
            oddsStats.BothTeamsToScore_No_Min = bothTeamsToScore_no_coefs.Length != 0 ? bothTeamsToScore_no_coefs.First() : 0;
            oddsStats.BothTeamsToScore_No_Max = bothTeamsToScore_no_coefs.Length != 0 ? bothTeamsToScore_no_coefs.Last() : 0;

            //total
            oddsStats.Total2_5Over_Average = total2_5_over_coefs.Length != 0 ? total2_5_over_coefs.Average() : 0;
            oddsStats.Total2_5Over_Min = total2_5_over_coefs.Length != 0 ? total2_5_over_coefs.First() : 0;
            oddsStats.Total2_5Over_Max = total2_5_over_coefs.Length != 0 ? total2_5_over_coefs.Last() : 0;

            oddsStats.Total2_5Under_Average = total2_5_under_coefs.Length != 0 ? total2_5_under_coefs.Average() : 0;
            oddsStats.Total2_5Under_Min = total2_5_under_coefs.Length != 0 ? total2_5_under_coefs.First() : 0;
            oddsStats.Total2_5Under_Max = total2_5_under_coefs.Length != 0 ? total2_5_under_coefs.Last() : 0;

            //ht - main odds
            oddsStats.HTHomeWinCoef_Average = ht_homeTeamWinsCoefs.Length != 0 ? ht_homeTeamWinsCoefs.Average() : 0;
            oddsStats.HTHomeWinCoef_Min = ht_homeTeamWinsCoefs.Length != 0 ? ht_homeTeamWinsCoefs.First() : 0;
            oddsStats.HTHomeWinCoef_Max = ht_homeTeamWinsCoefs.Length != 0 ? ht_homeTeamWinsCoefs.Last() : 0;

            oddsStats.HTAwayWinCoef_Average = ht_awayTeamWinsCoefs.Length != 0 ? ht_awayTeamWinsCoefs.Average() : 0;
            oddsStats.HTAwayWinCoef_Min = ht_awayTeamWinsCoefs.Length != 0 ? ht_awayTeamWinsCoefs.First() : 0;
            oddsStats.HTAwayWinCoef_Max = ht_awayTeamWinsCoefs.Length != 0 ? ht_awayTeamWinsCoefs.Last() : 0;

            oddsStats.HTDrawCoef_Average = ht_drawCoefs.Length != 0 ? ht_drawCoefs.Average() : 0;
            oddsStats.HTDrawCoef_Min = ht_drawCoefs.Length != 0 ? ht_drawCoefs.First() : 0;
            oddsStats.HTDrawCoef_Max = ht_drawCoefs.Length != 0 ? ht_drawCoefs.Last() : 0;

            oddsStats.LastUpdate = DateTime.Now;
        }
    }
}

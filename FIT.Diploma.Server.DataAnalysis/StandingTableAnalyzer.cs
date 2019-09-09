using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.LeagueData;
using FIT.Diploma.Server.Database.SystemData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIT.Diploma.Server.DataAnalysis
{
    public class StandingTableAnalyzer : IDataAnalyzer
    {
        public string GetTargetDataTable()
        {
            return "LeagueSeasonTeams";
        }

        private SystemDataRepository systemRepo = new SystemDataRepository();
        private LeagueDataRepository leagueRepo = new LeagueDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();

        public void RunAnalyzing()
        {
            var allSeasons = leagueRepo.GetAllLeagueSeasons(leagueRepo.GetLaLiga());
            foreach(var season in allSeasons)
            {
                var standingTable = systemRepo.GetStandingTable(season);
                if (!standingTable.AnalysisDone)
                    AnalyzeLeagueSeason(season, standingTable);
                else
                    Console.WriteLine($"Season: {season.LeagueSeasonId} [{season.StartYear}-{season.StartYear + 1}] have been already analyzed. Skip.");
            }
        }

        private void AnalyzeLeagueSeason(LeagueSeason season, StandingTableAnalysis standingTableRecord)
        {
            var allSeasonGames = leagueRepo.GetAllSeasonGames(season.LeagueSeasonId);
            var analyzedGames = systemRepo.GetStandingTableAnalyzedGames(season.LeagueSeasonId);
            var allTeams = allSeasonGames.Select(g => g.AwayTeamId)
                            .Union(allSeasonGames.Select(g => g.HomeTeamId))
                            .Distinct().OrderBy(i => i).ToList();

            var processedGames = new List<int>();
            var allSeasonTeams = analysisRepo.GetRangeOfTeams(allTeams, season);
            foreach(var game in allSeasonGames)
            {
                if (game.Result == 0 || analyzedGames.FindIndex(g => g.GameId == game.GameId) >= 0)  continue;
                var homeTeam = allSeasonTeams.Where(n => n.FootballTeamId == game.HomeTeamId).FirstOrDefault();
                var awayTeam = allSeasonTeams.Where(n => n.FootballTeamId == game.AwayTeamId).FirstOrDefault();

                homeTeam.GamePlayed++;
                awayTeam.GamePlayed++;

                homeTeam.GoalsFor += game.HomeTeamGoals;
                homeTeam.GoalsAgainst += game.AwayTeamGoals;
                awayTeam.GoalsFor += game.AwayTeamGoals;
                awayTeam.GoalsAgainst += game.HomeTeamGoals;
                
                if (game.Result == GameResult.HomeWin)
                {
                    homeTeam.WinsCount++;
                    awayTeam.LossesCount++;

                    homeTeam.Points += 3;
                }
                else if (game.Result == GameResult.AwayWin)
                {
                    awayTeam.WinsCount++;
                    homeTeam.LossesCount++;

                    awayTeam.Points += 3;
                }
                else
                {
                    awayTeam.DrawsCount++;
                    homeTeam.DrawsCount++;

                    awayTeam.Points += 1;
                    homeTeam.Points += 1;
                }

                processedGames.Add(game.GameId);
            }

            //set table places
            allSeasonTeams = allSeasonTeams.OrderBy(i => i.Points).ThenBy(i=> (i.GoalsFor - i.GoalsAgainst)).ToList();
            int place = 20;
            bool finishedSeason = true;
            foreach (var team in allSeasonTeams)
            {
                team.TablePlace = place;
                if (team.GamePlayed < 38) finishedSeason = false;
                Console.WriteLine($"[{place}] TeamID = {team.FootballTeamId}  GamePlayed = {team.GamePlayed} Points = {team.Points} GoalDiff = {team.GoalsFor - team.GoalsAgainst}");
                place--;
            }

            analysisRepo.UpdateSeasonTeams(allSeasonTeams);
            systemRepo.AddAnalyzedGames_StandingTable(processedGames, season.LeagueSeasonId);
            if (finishedSeason) {
                standingTableRecord.AnalysisDone = true;
                systemRepo.UpdateStandingTable(standingTableRecord);
            }
        }
    }
}

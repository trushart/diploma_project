using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.AnalyzerResults;
using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Linq;

namespace FIT.Diploma.Server.DataAnalysis
{
    public class FootballTeamFormAnalyzer : IDataAnalyzer
    {
        private ILeagueDataRepository leagueRepo = new LeagueDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();
        private SystemDataRepository systemRepo = new SystemDataRepository();

        public string GetTargetDataTable()
        {
            return "FootballTeamForm";
        }

        public void RunAnalyzing()
        {
            var allSeasons = leagueRepo.GetAllLeagueSeasons(leagueRepo.GetLaLiga());
            var analyzedSeasons = systemRepo.GetTeamFormAnalyzedSeasons();
            foreach (var season in allSeasons)
            {
                var analyzedSeasonId = analyzedSeasons.FindIndex(s => s.LeagueSeasonId == season.LeagueSeasonId);
                if (analyzedSeasonId >= 0 && analyzedSeasons[analyzedSeasonId].AnalysisFinished)
                {
                    Console.WriteLine($"Season {season.LeagueSeasonId} already analyzed.");
                    continue;
                }
                var seasonTeams = analysisRepo.GetSeasonTeams(season.LeagueSeasonId);

                foreach(var team in seasonTeams)
                {
                    AnalyzedTeam(team);
                }
                
                if (season.EndDate < DateTime.Now)
                    systemRepo.AddAnalyzedSeasons_TeamForm(season.LeagueSeasonId);
            }
        }

        private void AnalyzedTeam(LeagueSeasonTeams team)
        {
            var teamsSeasonGames = leagueRepo.GetAllSeasonGames(team.LeagueSeasonId)
                                        .Where(g => g.HomeTeamId == team.FootballTeamId || g.AwayTeamId == team.FootballTeamId);
            var teamForm5 = new FootballTeamForm
            {
                TeamId = team.FootballTeamId,
                LeagueSeasonId = team.LeagueSeasonId,
                TimePeriod = TimePeriod.Last5Matches
            };
            var teamForm10 = new FootballTeamForm
            {
                TeamId = team.FootballTeamId,
                LeagueSeasonId = team.LeagueSeasonId,
                TimePeriod = TimePeriod.Last10Matches
            };
            var teamFormAll = new FootballTeamForm
            {
                TeamId = team.FootballTeamId,
                LeagueSeasonId = team.LeagueSeasonId,
                TimePeriod = TimePeriod.AllMatches
            };

            var teamFormAllHome = new FootballTeamForm
            {
                TeamId = team.FootballTeamId,
                LeagueSeasonId = team.LeagueSeasonId,
                TimePeriod = TimePeriod.AllHomeMatches
            };

            var teamFormAllAway = new FootballTeamForm
            {
                TeamId = team.FootballTeamId,
                LeagueSeasonId = team.LeagueSeasonId,
                TimePeriod = TimePeriod.AllAwayMatches
            };

            var i = 0;

            foreach (var game in teamsSeasonGames)
            {
                if (game.Result == 0) continue;
                var gameTotal = game.HomeTeamGoals + game.AwayTeamGoals;
                ComputeTeamFormByGame(teamFormAll, game);
                if (i < 5) ComputeTeamFormByGame(teamForm5, game);
                if (i < 10) ComputeTeamFormByGame(teamForm10, game);

                if(game.HomeTeamId == team.FootballTeamId)
                    ComputeTeamFormByGame(teamFormAllHome, game);
                else
                    ComputeTeamFormByGame(teamFormAllAway, game);

                i++;
            }

            analysisRepo.AddOrUpdateTeamForm(teamForm5);
            analysisRepo.AddOrUpdateTeamForm(teamForm10);
            analysisRepo.AddOrUpdateTeamForm(teamFormAll);
            analysisRepo.AddOrUpdateTeamForm(teamFormAllHome);
            analysisRepo.AddOrUpdateTeamForm(teamFormAllAway);
        }

        private void ComputeTeamFormByGame(FootballTeamForm form, Game game)
        {
            bool isHomeGame = game.HomeTeamId == form.TeamId;
            var total = game.AwayTeamGoals + game.HomeTeamGoals;

            if (game.Result == GameResult.Draw)
            {
                form.DrawsCount++;
                form.Points += 1;
            }                
            else if (game.Result == GameResult.HomeWin)
            {
                form.WinsCount += isHomeGame ? 1 : 0;
                form.LossesCount += isHomeGame ? 0 : 1;
                form.Points += isHomeGame ? 3 : 0;
            }
            else
            {
                form.WinsCount += isHomeGame ? 0 : 1;
                form.LossesCount += isHomeGame ? 1 : 0;
                form.Points += isHomeGame ? 0 : 3;
            }
            form.GamePlayed++;

            form.Goals += total;
            form.GoalsFor += isHomeGame ? game.HomeTeamGoals : game.AwayTeamGoals;
            form.GoalsAgainst += isHomeGame ? game.AwayTeamGoals : game.HomeTeamGoals;

            if (total > 2.5)
                form.GamesOver2_5++;
            else
                form.GamesUnder2_5++;

            if (game.HomeTeamGoals > 0 && game.AwayTeamGoals > 0)
                form.BTTS_Yes++;
            else
                form.BTTS_No++;

            //per game
            form.WinsPercentage = (double)form.WinsCount / form.GamePlayed;
            form.DrawsPercentage = (double)form.DrawsCount / form.GamePlayed;
            form.LossesPercentage = (double)form.LossesCount / form.GamePlayed;

            form.GoalsPerGame = (double)form.Goals / form.GamePlayed;
            form.GoalsForPerGame = (double)form.GoalsFor / form.GamePlayed;
            form.GoalsAgainstPerGame = (double)form.GoalsAgainst / form.GamePlayed;

            form.LastUpdate = DateTime.Now;
        }
    }
}

using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Shared.DataAccess.Dto;
using Nancy;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using SharedRoutes = FIT.Diploma.Shared.Constants.Routes;

namespace FIT.Diploma.REST.Host.Modules
{
    public class LeagueSeasonModule : NancyModule
    {
        private LeagueDataRepository leagueRepo = new LeagueDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();

        public LeagueSeasonModule()
        {
            Get[SharedRoutes.StandingTable] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var season = leagueRepo.GetLeagueSeasonById(seasonId);
                var seasonTeams = analysisRepo.GetSeasonTeams(seasonId);
                var teamsDb = leagueRepo.GetFootballTeamsByIds(seasonTeams.Select(t => t.FootballTeamId).ToArray());
                var standingTable = new StandingTableDto
                {
                    LeagueSeasonId = seasonId,
                    StartDate = season.StartDate,
                    EndDate = season.EndDate,
                    IsFinished = season.EndDate < DateTime.Now,
                    Teams = seasonTeams.Select(Mapper.Map<LeagueSeasonTeamDto>).OrderBy(t => t.TablePlace).ToList()
                };
                foreach(var team in standingTable.Teams)
                {
                    var teamDb = teamsDb.Where(t => t.TeamId == team.FootballTeamId).FirstOrDefault();
                    team.FootballTeamName = teamDb.Name;
                }
                return Response.AsJson(standingTable);
            };

            Get[SharedRoutes.SeasonGames] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var seasonGames = leagueRepo.GetAllSeasonGames(seasonId).Select(Mapper.Map<GameDto>).ToList();
                return Response.AsJson(seasonGames);
            };

            Get[SharedRoutes.GameStats] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var matchId = (int)parameters.id;
                if (!leagueRepo.GetAllSeasonGames(seasonId).Any(g => g.GameId == matchId))
                    return HttpStatusCode.NotFound;
                bool onlyCreated;
                var gameStatsDb = leagueRepo.GetGameStats(matchId, out onlyCreated);
                var gameStats = Mapper.Map<GameInfoDto>(gameStatsDb);
                
                return Response.AsJson(gameStats);
            };

            Get[SharedRoutes.SeasonTotalStats] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var season = leagueRepo.GetLeagueSeasonById(seasonId);
                var seasonTeams = analysisRepo.GetSeasonTeams(seasonId);

                var teamsFormDb_allGames = analysisRepo.GetSeasonTeamForms_AllGames(seasonId);
                var teamsFormDb_homeGames = analysisRepo.GetSeasonTeamForms_HomeGames(seasonId);
                var teamsFormDb_awayGames = analysisRepo.GetSeasonTeamForms_AwayGames(seasonId);

                var teamTotals = new List<SeasonTeamTotalDto>();
                foreach(var team in seasonTeams)
                {
                    var teamFormDb_allGames = teamsFormDb_allGames.Where(t => t.TeamId == team.FootballTeamId).FirstOrDefault();
                    var teamFormDb_homeGames = teamsFormDb_homeGames.Where(t => t.TeamId == team.FootballTeamId).FirstOrDefault();
                    var teamFormDb_awayGames = teamsFormDb_awayGames.Where(t => t.TeamId == team.FootballTeamId).FirstOrDefault();

                    var teamTotal = new SeasonTeamTotalDto
                    {
                        FootballTeamId = team.FootballTeamId,
                        FootballTeamName = team.FootballTeam.Name,
                        GamePlayed = team.GamePlayed,
                        TotalOver = teamFormDb_allGames?.GamesOver2_5 ?? 0,
                        TotalUnder = teamFormDb_allGames?.GamesUnder2_5 ?? 0,
                        TotalOverHome = teamFormDb_homeGames?.GamesOver2_5 ?? 0,
                        TotalUnderHome = teamFormDb_homeGames?.GamesUnder2_5 ?? 0,
                        TotalOverAway = teamFormDb_awayGames?.GamesOver2_5 ?? 0,
                        TotalUnderAway = teamFormDb_awayGames?.GamesUnder2_5 ?? 0
                    };
                    teamTotals.Add(teamTotal);
                }

                var totalTable = new TotalTableDto
                {
                    LeagueSeasonId = seasonId,
                    StartDate = season.StartDate,
                    EndDate = season.EndDate,
                    IsFinished = season.EndDate < DateTime.Now,
                    Teams = teamTotals.OrderBy(t => t.TotalOver).ToList()
                };
                return Response.AsJson(totalTable);
            };

            Get[SharedRoutes.SeasonBttsStats] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var season = leagueRepo.GetLeagueSeasonById(seasonId);
                var seasonTeams = analysisRepo.GetSeasonTeams(seasonId);

                var teamsFormDb_allGames = analysisRepo.GetSeasonTeamForms_AllGames(seasonId);
                var teamsFormDb_homeGames = analysisRepo.GetSeasonTeamForms_HomeGames(seasonId);
                var teamsFormDb_awayGames = analysisRepo.GetSeasonTeamForms_AwayGames(seasonId);

                var teamTotals = new List<SeasonTeamBttsDto>();
                foreach (var team in seasonTeams)
                {
                    var teamFormDb_allGames = teamsFormDb_allGames.Where(t => t.TeamId == team.FootballTeamId).FirstOrDefault();
                    var teamFormDb_homeGames = teamsFormDb_homeGames.Where(t => t.TeamId == team.FootballTeamId).FirstOrDefault();
                    var teamFormDb_awayGames = teamsFormDb_awayGames.Where(t => t.TeamId == team.FootballTeamId).FirstOrDefault();

                    var teamTotal = new SeasonTeamBttsDto
                    {
                        FootballTeamId = team.FootballTeamId,
                        FootballTeamName = team.FootballTeam.Name,
                        GamePlayed = team.GamePlayed,
                        BttsYes = teamFormDb_allGames?.BTTS_Yes ?? 0,
                        BttsNo = teamFormDb_allGames?.BTTS_No ?? 0,
                        BttsYesHome = teamFormDb_homeGames?.BTTS_Yes ?? 0,
                        BttsNoHome = teamFormDb_homeGames?.BTTS_No ?? 0,
                        BttsYesAway = teamFormDb_awayGames?.BTTS_Yes ?? 0,
                        BttsNoAway = teamFormDb_awayGames?.BTTS_No ?? 0
                    };
                    teamTotals.Add(teamTotal);
                }

                var totalTable = new BttsTableDto
                {
                    LeagueSeasonId = seasonId,
                    StartDate = season.StartDate,
                    EndDate = season.EndDate,
                    IsFinished = season.EndDate < DateTime.Now,
                    Teams = teamTotals.OrderBy(t => t.BttsYes).ToList()
                };
                return Response.AsJson(totalTable);
            };
        }
    }
}

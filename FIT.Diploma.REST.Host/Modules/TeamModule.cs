using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Shared.DataAccess.Dto;
using Nancy;
using AutoMapper;
using System.Linq;
using SharedRoutes = FIT.Diploma.Shared.Constants.Routes;

namespace FIT.Diploma.REST.Host.Modules
{
    public class TeamModule : NancyModule
    {
        private LeagueDataRepository leagueRepo = new LeagueDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();

        public TeamModule()
        {
            Get[SharedRoutes.SeasonTeams] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var seasonTeams = analysisRepo.GetSeasonTeams(seasonId).Select(Mapper.Map<LeagueSeasonTeamDto>).OrderBy(t => t.TablePlace).ToList();
                return Response.AsJson(seasonTeams);
            };

            Get[SharedRoutes.TeamInfo] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var teamId = (int)parameters.teamId;
                var seasonTeam = analysisRepo.GetSeasonTeams(seasonId).Where(t => t.FootballTeamId == teamId)
                                    .Select(Mapper.Map<LeagueSeasonTeamDto>).OrderBy(t => t.TablePlace).FirstOrDefault();
                return Response.AsJson(seasonTeam);
            };

            Get[SharedRoutes.TeamFormByType] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var teamId = (int)parameters.teamId;
                var type = (int)parameters.type;
                FootballTeamFormDto teamForm;
                switch (type)
                {
                    case (int)TimePeriodDto.AllMatches:
                        teamForm = analysisRepo.GetSeasonTeamForms_AllGames(seasonId).Where(t => t.TeamId == teamId).Select(Mapper.Map<FootballTeamFormDto>).FirstOrDefault();
                        break;
                    case (int)TimePeriodDto.AllHomeMatches:
                        teamForm = analysisRepo.GetSeasonTeamForms_HomeGames(seasonId).Where(t => t.TeamId == teamId).Select(Mapper.Map<FootballTeamFormDto>).FirstOrDefault();
                        break;
                    case (int)TimePeriodDto.AllAwayMatches:
                        teamForm = analysisRepo.GetSeasonTeamForms_AwayGames(seasonId).Where(t => t.TeamId == teamId).Select(Mapper.Map<FootballTeamFormDto>).FirstOrDefault();
                        break;
                    default:
                        return HttpStatusCode.PreconditionFailed;
                }
                return Response.AsJson(teamForm);
            };

            Get[SharedRoutes.TeamForm5] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var teamId = (int)parameters.teamId;
                var teamFormDb = analysisRepo.GetFootballTeamFormByIdAndType(seasonId, teamId, Server.Database.AnalyzerResults.TimePeriod.Last5Matches);
                var teamForm = Mapper.Map<FootballTeamFormDto>(teamFormDb);
                return Response.AsJson(teamForm);
            };

            Get[SharedRoutes.TeamForm10] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var teamId = (int)parameters.teamId;
                var teamFormDb = analysisRepo.GetFootballTeamFormByIdAndType(seasonId, teamId, Server.Database.AnalyzerResults.TimePeriod.Last10Matches);
                var teamForm = Mapper.Map<FootballTeamFormDto>(teamFormDb);
                return Response.AsJson(teamForm);
            };

            Get[SharedRoutes.HeadToHeadStats] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var team1Id = (int)parameters.team1Id;
                var team2Id = (int)parameters.team2Id;
                var h2hDb = analysisRepo.GetTeamsHeadToHead(team1Id, team2Id);
                var h2h = Mapper.Map<HeadToHeadDto>(h2hDb);
                return Response.AsJson(h2h);
            };
        }
    }
}

using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Shared.DataAccess.Dto;
using Nancy;
using AutoMapper;
using System.Linq;
using SharedRoutes = FIT.Diploma.Shared.Constants.Routes;

namespace FIT.Diploma.REST.Host.Modules
{
    public class LeagueRoundModule : NancyModule
    {
        private LeagueDataRepository leagueRepo = new LeagueDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();

        public LeagueRoundModule()
        {
            Get[SharedRoutes.SeasonAllRounds] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var rounds = leagueRepo.GetCurrentSeasonRounds(seasonId).Select(Mapper.Map<RoundDto>).ToList();
                return Response.AsJson(rounds);
            };

            Get[SharedRoutes.RoundGames] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var roundId = (int)parameters.roundId;
                var gameRoundsDb = leagueRepo.GetAllRoundGames(roundId);
                if (gameRoundsDb.Any(g => g.SeasonRound.LeagueSeasonId != seasonId))
                    return HttpStatusCode.Conflict;
                var games = gameRoundsDb.Select(Mapper.Map<GameDto>).ToList();
                return Response.AsJson(games);
            };

            Get[SharedRoutes.CurrentRoundGames] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var season = leagueRepo.GetLeagueSeasonById(seasonId);
                var currentRound = leagueRepo.GetCurrentSeasonRound(season);
                var gameRoundsDb = leagueRepo.GetAllRoundGames(currentRound.RoundId);
                if (gameRoundsDb.Any(g => g.SeasonRound.LeagueSeasonId != seasonId))
                    return HttpStatusCode.Conflict;
                var games = gameRoundsDb.Select(Mapper.Map<GameDto>).ToList();
                return Response.AsJson(games);
            };

            Get[SharedRoutes.SeasonRoundsTable] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var rounds = leagueRepo.GetCurrentSeasonRounds(seasonId).Select(Mapper.Map<RoundStatsDto>).OrderBy(r => r.RoundNumber).ToList();
                var roundTable = new RoundsTable
                {
                    LeagueSeasonId = seasonId,
                    IsFinished = rounds.Count >= 38,
                    Rounds = rounds,
                    StartDate = rounds.FirstOrDefault()?.StartDate,
                    EndDate = rounds.LastOrDefault()?.EndDate,
                };
                return Response.AsJson(roundTable);
            };

            Get[SharedRoutes.SeasonRoundsStats] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var averageRoundStats = Mapper.Map<SeasonAverageRoundDto>(analysisRepo.GetAverageRoundStats(seasonId));
                return Response.AsJson(averageRoundStats);
            };

        }
    }
}

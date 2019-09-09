using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Shared.DataAccess.Dto;
using Nancy;
using AutoMapper;
using System.Linq;
using SharedRoutes = FIT.Diploma.Shared.Constants.Routes;

namespace FIT.Diploma.REST.Host.Modules
{
    public class PredictionModule : NancyModule
    {
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();
        private LeagueDataRepository leagueRepo = new LeagueDataRepository();

        public PredictionModule()
        {
            Get[SharedRoutes.CurrentPredictions] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var predictions = analysisRepo.GetSeasonPredictions(seasonId).Where(p => p.IsFinished == false)
                                                .Select(Mapper.Map<PredictionDto>).ToList();
                var seasonDb = leagueRepo.GetLeagueSeasonById(seasonId);
                var result = new PredictionsResponseDto
                {
                    Predictions = predictions,
                    LeagueId = seasonDb.League.LeagueId,
                    LeagueSeasonId = seasonId,
                    StartYear = seasonDb.StartYear
                };
                return Response.AsJson(result);
            };

            Get[SharedRoutes.AllPredictions] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var predictions = analysisRepo.GetSeasonPredictions(seasonId).Select(Mapper.Map<PredictionDto>).ToList();
                var seasonDb = leagueRepo.GetLeagueSeasonById(seasonId);
                var result = new PredictionsResponseDto
                {
                    Predictions = predictions,
                    LeagueId = seasonDb.League.LeagueId,
                    LeagueSeasonId = seasonId,
                    StartYear = seasonDb.StartYear
                };
                return Response.AsJson(result);
            };

            Get[SharedRoutes.FinishedPredictions] = parameters => {
                var seasonId = (int)parameters.seasonId;
                var predictions = analysisRepo.GetSeasonPredictions(seasonId).Where(p => p.IsFinished == true)
                                                .Select(Mapper.Map<PredictionDto>).ToList();
                var seasonDb = leagueRepo.GetLeagueSeasonById(seasonId);
                var result = new PredictionsResponseDto
                {
                    Predictions = predictions,
                    LeagueId = seasonDb.League.LeagueId,
                    LeagueSeasonId = seasonId,
                    StartYear = seasonDb.StartYear
                };
                return Response.AsJson(result);
            };
        }
    }
}

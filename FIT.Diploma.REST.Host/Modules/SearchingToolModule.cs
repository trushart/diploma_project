using FIT.Diploma.Shared.DataAccess.Dto;
using FIT.Diploma.Server.DataAnalysis.SearchingTool;
using Nancy;
using AutoMapper;
using System.Linq;
using Nancy.ModelBinding;
using SharedRoutes = FIT.Diploma.Shared.Constants.Routes;

namespace FIT.Diploma.REST.Host.Modules
{
    public class SearchingToolModule : NancyModule
    {
        ISearchEngine searchEngine = new SearchEngine();

        public SearchingToolModule()
        {
            Post[SharedRoutes.SearchingToolGames] = parameters => {
                var conditions = this.Bind<SearchToolConditionsDto>();
                var conditionsAnalysis = Mapper.Map<SearchConditions>(conditions);
                var games = searchEngine.GetGames(conditionsAnalysis).Select(Mapper.Map<GameDto>).ToList();
                return Response.AsJson(games);
            };

            Post[SharedRoutes.SearchingToolGamesNumber] = parameters => {
                var conditions = this.Bind<SearchToolConditionsDto>();
                var conditionsAnalysis = Mapper.Map<SearchConditions>(conditions);
                var gamesNumber = searchEngine.GetGamesNumber(conditionsAnalysis);
                return Response.AsJson(gamesNumber);
            };

            Post[SharedRoutes.SearchingToolMaxStreak] = parameters => {
                var conditions = this.Bind<SearchToolConditionsDto>();
                var conditionsAnalysis = Mapper.Map<SearchConditions>(conditions);
                var games = searchEngine.GetMaximalStreak(conditionsAnalysis).Select(Mapper.Map<GameDto>).ToList();
                return Response.AsJson(games);
            };

            Post[SharedRoutes.SearchingToolMinStreak] = parameters => {
                var conditions = this.Bind<SearchToolConditionsDto>();
                var conditionsAnalysis = Mapper.Map<SearchConditions>(conditions);
                var games = searchEngine.GetMinimalStreak(conditionsAnalysis).Select(Mapper.Map<GameDto>).ToList();
                return Response.AsJson(games);
            };

            Post[SharedRoutes.SearchingToolStreakNumber] = parameters => {
                var conditions = this.Bind<SearchToolConditionsDto>();
                var conditionsAnalysis = Mapper.Map<SearchConditions>(conditions);
                var streakConditionsAnalysis = Mapper.Map<StreakConditions>(conditions.StreakConditions);
                var gamesNumber = searchEngine.GetNumberOfStreaks(conditionsAnalysis, streakConditionsAnalysis);
                return Response.AsJson(gamesNumber);
            };
        }
    }
}

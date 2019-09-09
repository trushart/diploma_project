using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Shared.DataAccess.Dto;
using Nancy;
using AutoMapper;
using System.Linq;
using SharedRoutes = FIT.Diploma.Shared.Constants.Routes;
using System;

namespace FIT.Diploma.REST.Host
{
    public class LeagueModule : NancyModule
    {
        private LeagueDataRepository leagueRepo = new LeagueDataRepository();

        public LeagueModule()
        {
            Get[SharedRoutes.AllLeagues] = x => {
                var model = leagueRepo.GetAllLeagues().Select(Mapper.Map<LeagueDto>);
                return Response.AsJson(model);
            };

            Get[SharedRoutes.AllLeagueSeasons] = parameters => {
                var leagueId = (int)parameters.leagueId;
                var model = leagueRepo.GetAllLeagueSeasons(leagueId).Select(Mapper.Map<LeagueSeasonDto>).OrderBy(x => x.StartYear);
                return Response.AsJson(model);
            };

            Get[SharedRoutes.LeagueInfo] = parameters => {
                var leagueId = (int)parameters.leagueId;
                var model = leagueRepo.GetAllLeagues().Where(l => l.LeagueId == leagueId).Select(Mapper.Map<LeagueInfoDto>).OrderBy(x => x.LeagueName);
                return Response.AsJson(model);
            };
        }
    }
}

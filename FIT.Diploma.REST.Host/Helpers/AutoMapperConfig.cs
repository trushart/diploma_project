using AutoMapper;
using FIT.Diploma.Server.Database.LeagueData;
using FIT.Diploma.Server.Database.AnalyzerResults;
using FIT.Diploma.Shared.DataAccess.Dto;

namespace FIT.Diploma.REST.Host.Helpers
{
    public static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<HeadToHeadStats, HeadToHeadDto>()
                    .ForMember(
                        dest => dest.Team1Name, opt => { opt.MapFrom(src => src.FootballTeam1.Name); }
                    )
                    .ForMember(
                        dest => dest.Team2Name, opt => { opt.MapFrom(src => src.FootballTeam2.Name); }
                    );

                cfg.CreateMap<Game, GameDto>()
                    .ForMember(
                        dest => dest.HomeTeamName, opt => { opt.MapFrom(src => src.HomeFootballTeam.Name); }
                    )
                    .ForMember(
                        dest => dest.AwayTeamName, opt => { opt.MapFrom(src => src.AwayFootballTeam.Name); }
                    );

                cfg.CreateMap<FootballTeamForm, FootballTeamFormDto>()
                    .ForMember(
                        dest => dest.TeamName, opt => { opt.MapFrom(src => src.FootballTeam.Name); }
                    );

                cfg.CreateMap<GameStats, GameInfoDto>()
                    .ForMember(
                        dest => dest.HomeTeamId, opt => { opt.MapFrom(src => src.Game.HomeTeamId); }
                    )
                    .ForMember(
                        dest => dest.AwayTeamId, opt => { opt.MapFrom(src => src.Game.AwayTeamId); }
                    )
                    .ForMember(
                        dest => dest.HomeTeamGoals, opt => { opt.MapFrom(src => src.Game.HomeTeamGoals); }
                    )
                    .ForMember(
                        dest => dest.AwayTeamGoals, opt => { opt.MapFrom(src => src.Game.AwayTeamGoals); }
                    )
                    .ForMember(
                        dest => dest.Date, opt => { opt.MapFrom(src => src.Game.Date); }
                    )
                    .ForMember(
                        dest => dest.Result, opt => { opt.MapFrom(src => src.Game.Result); }
                    )
                    .ForMember(
                        dest => dest.Stadium, opt => { opt.MapFrom(src => src.Game.Stadium); }
                    )
                    .ForMember(
                        dest => dest.RoundId, opt => { opt.MapFrom(src => src.Game.RoundId); }
                    )
                    .ForMember(
                        dest => dest.HomeTeamName, opt => { opt.MapFrom(src => src.Game.HomeFootballTeam.Name); }
                    )
                    .ForMember(
                        dest => dest.AwayTeamName, opt => { opt.MapFrom(src => src.Game.AwayFootballTeam.Name); }
                    );

                cfg.CreateMap<FootballLeague, LeagueDto>()
                    .ForMember(
                        dest => dest.LeagueName, opt => { opt.MapFrom(src => src.Name); }
                    )
                    .ForMember(
                        dest => dest.Location, opt => { opt.MapFrom(src => src.Location.Country); }
                    );

                cfg.CreateMap<FootballLeague, LeagueInfoDto>()
                    .ForMember(
                        dest => dest.LeagueName, opt => { opt.MapFrom(src => src.Name); }
                    )
                    .ForMember(
                        dest => dest.Location, opt => { opt.MapFrom(src => src.Location.Country); }
                    )
                    .ForMember(
                        dest => dest.LeagueInfo, opt => { opt.MapFrom(src => src.MoreInformation); }
                    );

                cfg.CreateMap<LeagueSeason, LeagueSeasonDto>()
                    .ForMember(
                        dest => dest.LeagueId, opt => { opt.MapFrom(src => src.League.LeagueId); }
                    );
            });
        }
    }
}

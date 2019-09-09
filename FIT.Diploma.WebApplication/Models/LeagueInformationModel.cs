using System.Collections.Generic;
using FIT.Diploma.Shared.DataAccess.Dto;

namespace FIT.Diploma.WebApplication.Models
{
    public class LeagueInformationModel
    {
        public string Error { get; set; }

        public List<LeagueDto> Leagues { get; set; }
        public LeagueDto SelectedLeague { get; set; }

        public List<LeagueSeasonDto> Seasons { get; set; }
        public LeagueSeasonDto SelectedSeason { get; set; }

        //Current round info
        public List<GameDto> CurrentRoundGames { get; set; }
        public RoundDto CurrentRound { get; set; }

        //Previous round info
        public List<GameDto> PreviousRoundGames { get; set; }
        public RoundDto PreviousRound { get; set; }

        public StandingTableDto StandingTable { get; set; }
    }
}
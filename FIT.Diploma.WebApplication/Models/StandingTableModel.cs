using FIT.Diploma.Shared.DataAccess.Dto;

namespace FIT.Diploma.WebApplication.Models
{
    public class StandingTableModel
    {
        public LeagueDto League { get; set; }
        public LeagueSeasonDto Season { get; set; }
        public StandingTableDto StandingTable { get; set; }
    }
}
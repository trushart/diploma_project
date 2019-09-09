using System;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class LeagueSeasonDto
    {
        public int LeagueSeasonId { get; set; }

        public int StartYear { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CountOfTeams { get; set; }

        public int CountOfRounds { get; set; }

        public int LeagueId { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class TotalTableDto
    {
        public int LeagueSeasonId { get; set; }
        public List<SeasonTeamTotalDto> Teams { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsFinished { get; set; }
    }

    [Serializable]
    public class SeasonTeamTotalDto
    {
        public int FootballTeamId { get; set; }
        public string FootballTeamName { get; set; }

        public int GamePlayed { get; set; }

        public int TotalOver { get; set; }
        public int TotalUnder { get; set; }

        public int TotalOverHome { get; set; }
        public int TotalUnderHome { get; set; }

        public int TotalOverAway { get; set; }
        public int TotalUnderAway { get; set; }
    }
}

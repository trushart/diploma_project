using System;
using System.Collections.Generic;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class StandingTableDto
    {
        public int LeagueSeasonId { get; set; }

        public List<LeagueSeasonTeamDto> Teams { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsFinished { get; set; }
    }

    [Serializable]
    public class LeagueSeasonTeamDto
    {
        public int FootballTeamId { get; set; }

        public string FootballTeamName { get; set; }

        public int TablePlace { get; set; }

        public int GamePlayed { get; set; }

        public int WinsCount { get; set; }

        public int DrawsCount { get; set; }

        public int LossesCount { get; set; }

        public int GoalsFor { get; set; }

        public int GoalsAgainst { get; set; }

        public int Points { get; set; }
    }
}

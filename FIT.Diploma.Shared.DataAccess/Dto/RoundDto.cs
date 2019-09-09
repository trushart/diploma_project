using System;
using System.Collections.Generic;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class RoundDto
    {
        public int RoundId { get; set; }
        public int LeagueSeasonId { get; set; }

        public int RoundNumber { get; set; }
        public int GamePlayed { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public bool IsFinished { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    [Serializable]
    public class RoundStatsDto : RoundDto
    {
        public int HomeWinsCount { get; set; }
        public int DrawsCount { get; set; }
        public int AwayWinsCount { get; set; }

        public int Goals { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }

        public int GamesOver2_5 { get; set; }
        public int GamesUnder2_5 { get; set; }

        public int BTTS_Yes { get; set; }
        public int BTTS_No { get; set; }
    }

    [Serializable]
    public class RoundsTable
    {
        public int LeagueSeasonId { get; set; }

        public List<RoundStatsDto> Rounds { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsFinished { get; set; }
    }
}

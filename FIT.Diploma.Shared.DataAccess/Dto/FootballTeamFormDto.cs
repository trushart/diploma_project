using System;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class FootballTeamFormDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public int LeagueSeasonId { get; set; }
        public TimePeriodDto TimePeriod { get; set; }

        public int GamePlayed { get; set; }
        public int WinsCount { get; set; }
        public int DrawsCount { get; set; }
        public int LossesCount { get; set; }

        public double WinsPercentage { get; set; }
        public double DrawsPercentage { get; set; }
        public double LossesPercentage { get; set; }

        public int Goals { get; set; }
        public double GoalsPerGame { get; set; }
        public int GoalsFor { get; set; }
        public double GoalsForPerGame { get; set; }
        public int GoalsAgainst { get; set; }
        public double GoalsAgainstPerGame { get; set; }


        public int GamesOver2_5 { get; set; }
        public int GamesUnder2_5 { get; set; }

        public int BTTS_Yes { get; set; }
        public int BTTS_No { get; set; }

        public int Points { get; set; }

        public DateTime LastUpdate { get; set; }
    }

    [Serializable]
    public enum TimePeriodDto
    {
        Last5Matches = 1,
        Last10Matches = 2,
        AllMatches = 3,
        AllHomeMatches = 4,
        AllAwayMatches = 5
    }
}

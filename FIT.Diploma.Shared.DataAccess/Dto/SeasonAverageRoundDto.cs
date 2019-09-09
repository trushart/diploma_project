using System;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class SeasonAverageRoundDto
    {
        public int LeagueSeasonId { get; set; }

        public double HomeWinsCount_Average { get; set; }
        public double DrawsCount_Average { get; set; }
        public double AwayWinsCount_Average { get; set; }
        public double HomeWinsCount_Max { get; set; }
        public double DrawsCount_Max { get; set; }
        public double AwayWinsCount_Max { get; set; }
        public double HomeWinsCount_Min { get; set; }
        public double DrawsCount_Min { get; set; }
        public double AwayWinsCount_Min { get; set; }

        public double Goals_Average { get; set; }
        public double HomeGoals_Average { get; set; }
        public double AwayGoals_Average { get; set; }
        public double Goals_Max { get; set; }
        public double HomeGoals_Max { get; set; }
        public double AwayGoals_Max { get; set; }
        public double Goals_Min { get; set; }
        public double HomeGoals_Min { get; set; }
        public double AwayGoals_Min { get; set; }

        public double GamesOver2_5_Average { get; set; }
        public double GamesUnder2_5_Average { get; set; }
        public double GamesOver2_5_Max { get; set; }
        public double GamesUnder2_5_Max { get; set; }
        public double GamesOver2_5_Min { get; set; }
        public double GamesUnder2_5_Min { get; set; }

        public double BTTS_Yes_Average { get; set; }
        public double BTTS_No_Average { get; set; }
        public double BTTS_Yes_Max { get; set; }
        public double BTTS_No_Max { get; set; }
        public double BTTS_Yes_Min { get; set; }
        public double BTTS_No_Min { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}

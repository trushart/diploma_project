using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FIT.Diploma.Server.Database.LeagueData;

namespace FIT.Diploma.Server.Database.AnalyzerResults
{
    public class AverageRoundStats
    {
        [Key]
        [ForeignKey("LeagueSeason")]
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

        public void Copy(AverageRoundStats stats)
        {
            if (LeagueSeasonId != stats.LeagueSeasonId)
                return;
            HomeWinsCount_Average = stats.HomeWinsCount_Average;
            DrawsCount_Average = stats.DrawsCount_Average;
            AwayWinsCount_Average = stats.AwayWinsCount_Average;
            HomeWinsCount_Min = stats.HomeWinsCount_Min;
            DrawsCount_Min = stats.DrawsCount_Min;
            AwayWinsCount_Min = stats.AwayWinsCount_Min;
            HomeWinsCount_Max = stats.HomeWinsCount_Max;
            DrawsCount_Max = stats.DrawsCount_Max;
            AwayWinsCount_Max = stats.AwayWinsCount_Max;

            Goals_Average = stats.Goals_Average;
            HomeGoals_Average = stats.HomeGoals_Average;
            AwayGoals_Average = stats.AwayGoals_Average;
            Goals_Min = stats.Goals_Min;
            HomeGoals_Min = stats.HomeGoals_Min;
            AwayGoals_Min = stats.AwayGoals_Min;
            Goals_Max = stats.Goals_Max;
            HomeGoals_Max = stats.HomeGoals_Max;
            AwayGoals_Max = stats.AwayGoals_Max;

            GamesOver2_5_Average = stats.GamesOver2_5_Average;
            GamesUnder2_5_Average = stats.GamesUnder2_5_Average;
            GamesOver2_5_Min = stats.GamesOver2_5_Min;
            GamesUnder2_5_Min = stats.GamesUnder2_5_Min;
            GamesOver2_5_Max = stats.GamesOver2_5_Max;
            GamesUnder2_5_Max = stats.GamesUnder2_5_Max;

            BTTS_Yes_Average = stats.BTTS_Yes_Average;
            BTTS_No_Average = stats.BTTS_No_Average;
            BTTS_Yes_Min = stats.BTTS_Yes_Min;
            BTTS_No_Min = stats.BTTS_No_Min;
            BTTS_Yes_Max = stats.BTTS_Yes_Max;
            BTTS_No_Max = stats.BTTS_No_Max;

            LastUpdate = DateTime.Now;
        }

        public virtual LeagueSeason LeagueSeason { get; set; }
    }
}

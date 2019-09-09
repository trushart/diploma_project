using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FIT.Diploma.Server.Database.LeagueData;

namespace FIT.Diploma.Server.Database.AnalyzerResults
{
    public class SeasonStats
    {
        [Key]
        [ForeignKey("LeagueSeason")]
        public int LeagueSeasonId { get; set; }

        public int GamePlayed { get; set; }

        public int HomeWinsCount { get; set; }
        public int DrawsCount { get; set; }
        public int AwayWinsCount { get; set; }

        public double HomeWinsPercentage { get; set; }
        public double DrawsPercentage { get; set; }
        public double AwayWinsPercentage { get; set; }

        public int Goals { get; set; }
        public int HomeTeamsGoals { get; set; }
        public int AwayTeamsGoals { get; set; }
        public double GoalsPerGame { get; set; }

        public int GamesOver2_5 { get; set; }
        public int GamesUnder2_5 { get; set; }

        public int BTTS_Yes { get; set; }
        public int BTTS_No { get; set; }

        public void Copy(SeasonStats stats)
        {
            if (LeagueSeasonId != stats.LeagueSeasonId) return;

            GamePlayed = stats.GamePlayed;

            HomeWinsCount = stats.HomeWinsCount;
            DrawsCount = stats.DrawsCount;
            AwayWinsCount = stats.AwayWinsCount;

            HomeWinsPercentage = stats.HomeWinsPercentage;
            DrawsPercentage = stats.DrawsPercentage;
            AwayWinsPercentage = stats.AwayWinsPercentage;

            Goals = stats.Goals;
            HomeTeamsGoals = stats.HomeTeamsGoals;
            AwayTeamsGoals = stats.AwayTeamsGoals;
            GoalsPerGame = stats.GoalsPerGame;

            GamesOver2_5 = stats.GamesOver2_5;
            GamesUnder2_5 = stats.GamesUnder2_5;

            BTTS_Yes = stats.BTTS_Yes;
            BTTS_No = stats.BTTS_No;

            LastUpdate = DateTime.Now;
        }

        public DateTime LastUpdate { get; set; }

        public virtual LeagueSeason LeagueSeason { get; set; }

    }
}

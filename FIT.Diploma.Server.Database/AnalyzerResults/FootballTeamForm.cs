using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FIT.Diploma.Server.Database.LeagueData;

namespace FIT.Diploma.Server.Database.AnalyzerResults
{
    public class FootballTeamForm
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("FootballTeam")]
        public int TeamId { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("LeagueSeason")]
        public int LeagueSeasonId { get; set; }

        [Key]
        [Column(Order = 3)]
        public TimePeriod TimePeriod { get; set; }

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

        public void Copy(FootballTeamForm teamForm)
        {
            if (TeamId != teamForm.TeamId || LeagueSeasonId != teamForm.LeagueSeasonId || TimePeriod != teamForm.TimePeriod)
                return;

            GamePlayed = teamForm.GamePlayed;
            WinsCount = teamForm.WinsCount;
            DrawsCount = teamForm.DrawsCount;
            LossesCount = teamForm.LossesCount;

            WinsPercentage = teamForm.WinsPercentage;
            DrawsPercentage = teamForm.DrawsPercentage;
            LossesPercentage = teamForm.LossesPercentage;

            Goals = teamForm.Goals;
            GoalsPerGame = teamForm.GoalsPerGame;
            GoalsFor = teamForm.GoalsFor;
            GoalsForPerGame = teamForm.GoalsForPerGame;
            GoalsAgainst = teamForm.GoalsAgainst;
            GoalsAgainstPerGame = teamForm.GoalsAgainstPerGame;

            GamesOver2_5 = teamForm.GamesOver2_5;
            GamesUnder2_5 = teamForm.GamesUnder2_5;

            BTTS_Yes = teamForm.BTTS_Yes;
            BTTS_No = teamForm.BTTS_No;

            Points = teamForm.Points;

            LastUpdate = DateTime.Now;
        }

        public virtual FootballTeam FootballTeam { get; set; }
        public virtual LeagueSeason LeagueSeason { get; set; }
    }

    public enum TimePeriod
    {
        Last5Matches = 1,
        Last10Matches = 2,
        AllMatches = 3,
        AllHomeMatches = 4,
        AllAwayMatches = 5
    }
}

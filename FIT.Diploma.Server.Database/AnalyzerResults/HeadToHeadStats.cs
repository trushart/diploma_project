using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FIT.Diploma.Server.Database.LeagueData;

namespace FIT.Diploma.Server.Database.AnalyzerResults
{
    public class HeadToHeadStats
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("FootballTeam1")]
        public int Team1Id { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("FootballTeam2")]
        public int Team2Id { get; set; }

        public int GamePlayed { get; set; }
        public int Team1WinsCount { get; set; }
        public int DrawsCount { get; set; }
        public int Team2WinsCount { get; set; }

        public double Team1WinsPercentage { get; set; }
        public double DrawsPercentage { get; set; }
        public double Team2WinsPercentage { get; set; }

        public int Goals { get; set; }
        public double GoalsPerGame { get; set; }
        public int Team1Goals { get; set; }
        public double Team1GoalsPerGame { get; set; }
        public int Team2Goals { get; set; }
        public double Team2GoalsPerGame { get; set; }


        public int GamesOver2_5 { get; set; }
        public int GamesUnder2_5 { get; set; }

        public int BTTS_Yes { get; set; }
        public int BTTS_No { get; set; }

        public DateTime LastUpdate { get; set; }

        public void Copy(HeadToHeadStats stats)
        {
            if (Team1Id != stats.Team1Id || Team2Id != stats.Team2Id)
                return;

            GamePlayed = stats.GamePlayed;
            Team1WinsCount = stats.Team1WinsCount;
            DrawsCount = stats.DrawsCount;
            Team2WinsCount = stats.Team2WinsCount;

            Team1WinsPercentage = stats.Team1WinsPercentage;
            DrawsPercentage = stats.DrawsPercentage;
            Team2WinsPercentage = stats.Team2WinsPercentage;

            Goals = stats.Goals;
            GoalsPerGame = stats.GoalsPerGame;
            Team1Goals = stats.Team1Goals;
            Team1GoalsPerGame = stats.Team1GoalsPerGame;
            Team2Goals = stats.Team2Goals;
            Team2GoalsPerGame = stats.Team2GoalsPerGame;

            GamesOver2_5 = stats.GamesOver2_5;
            GamesUnder2_5 = stats.GamesUnder2_5;

            BTTS_Yes = stats.BTTS_Yes;
            BTTS_No = stats.BTTS_No;

            LastUpdate = DateTime.Now;
        }

        public virtual FootballTeam FootballTeam1 { get; set; }
        public virtual FootballTeam FootballTeam2 { get; set; }
    }
}

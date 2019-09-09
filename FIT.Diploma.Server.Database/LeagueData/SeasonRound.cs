using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.LeagueData
{
    public class SeasonRound
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoundId { get; set; }

        [ForeignKey("LeagueSeason")]
        public int LeagueSeasonId { get; set; }

        public int RoundNumber { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int GamePlayed { get; set; }

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


        public bool IsFinished { get; set; }
        public DateTime LastUpdate { get; set; }

        public void Copy(SeasonRound round)
        {
            if (RoundId != round.RoundId)
                return;

            RoundNumber = round.RoundNumber;

            StartDate = round.StartDate;
            EndDate = round.EndDate;

            GamePlayed = round.GamePlayed;

            HomeWinsCount = round.HomeWinsCount;
            DrawsCount = round.DrawsCount;
            AwayWinsCount = round.AwayWinsCount;

            Goals = round.Goals;
            HomeGoals = round.HomeGoals;
            AwayGoals = round.AwayGoals;

            GamesOver2_5 = round.GamesOver2_5;
            GamesUnder2_5 = round.GamesUnder2_5;

            BTTS_Yes = round.BTTS_Yes;
            BTTS_No = round.BTTS_No;

            IsFinished = round.IsFinished;
            LastUpdate = DateTime.Now;
        }

        public virtual LeagueSeason LeagueSeason { get; set; }
    }
}

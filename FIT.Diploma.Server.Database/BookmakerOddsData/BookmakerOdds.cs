using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FIT.Diploma.Server.Database.LeagueData;

namespace FIT.Diploma.Server.Database.BookmakerOddsData
{
    public class BookmakerOdds
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }

        [ForeignKey("Game")]
        public int GameId { get; set; }

        [ForeignKey("Bookmaker")]
        public int BookmakerId { get; set; }

        public double HomeWinCoef { get; set; }
        public double DrawCoef { get; set; }
        public double AwayWinCoef { get; set; }

        public double DoubleChanceCoef_1X { get; set; }
        public double DoubleChanceCoef_12 { get; set; }
        public double DoubleChanceCoef_X2 { get; set; }

        public double BothTeamsToScore_Yes { get; set; }
        public double BothTeamsToScore_No { get; set; }

        public double Total2_5Over { get; set; }
        public double Total2_5Under{ get; set; }

        //HT = Half-Time
        public double HTHomeWinCoef { get; set; }
        public double HTDrawCoef { get; set; }
        public double HTAwayWinCoef { get; set; }

        public DateTime CreatedTime { get; set; }

        public virtual Bookmaker Bookmaker { get; set; }

        public virtual Game Game { get; set; }

        public void Copy(BookmakerOdds bookmakerOdds)
        {
            HomeWinCoef = bookmakerOdds.HomeWinCoef;
            DrawCoef = bookmakerOdds.DrawCoef;
            AwayWinCoef = bookmakerOdds.AwayWinCoef;

            DoubleChanceCoef_1X = bookmakerOdds.DoubleChanceCoef_1X;
            DoubleChanceCoef_12 = bookmakerOdds.DoubleChanceCoef_12;
            DoubleChanceCoef_X2 = bookmakerOdds.DoubleChanceCoef_X2;

            BothTeamsToScore_Yes = bookmakerOdds.BothTeamsToScore_Yes;
            BothTeamsToScore_No = bookmakerOdds.BothTeamsToScore_No;

            Total2_5Over = bookmakerOdds.Total2_5Over;
            Total2_5Under = bookmakerOdds.Total2_5Under;

            HTHomeWinCoef = bookmakerOdds.HTHomeWinCoef;
            HTDrawCoef = bookmakerOdds.HTDrawCoef;
            HTAwayWinCoef = bookmakerOdds.HTAwayWinCoef;

            CreatedTime = DateTime.Now;
        }
    }
}

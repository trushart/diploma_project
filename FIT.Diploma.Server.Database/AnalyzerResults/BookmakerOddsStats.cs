using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FIT.Diploma.Server.Database.LeagueData;

namespace FIT.Diploma.Server.Database.AnalyzerResults
{
    public class BookmakerOddsStats
    {
        [Key]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        //
        public double HomeWinCoef_Average { get; set; }
        public double DrawCoef_Average { get; set; }
        public double AwayWinCoef_Average { get; set; }
        public double HomeWinCoef_Min { get; set; }
        public double DrawCoef_Min { get; set; }
        public double AwayWinCoef_Min { get; set; }
        public double HomeWinCoef_Max { get; set; }
        public double DrawCoef_Max { get; set; }
        public double AwayWinCoef_Max { get; set; }


        public double DoubleChanceCoef_1X_Average { get; set; }
        public double DoubleChanceCoef_12_Average { get; set; }
        public double DoubleChanceCoef_X2_Average { get; set; }
        public double DoubleChanceCoef_1X_Min { get; set; }
        public double DoubleChanceCoef_12_Min { get; set; }
        public double DoubleChanceCoef_X2_Min { get; set; }
        public double DoubleChanceCoef_1X_Max { get; set; }
        public double DoubleChanceCoef_12_Max { get; set; }
        public double DoubleChanceCoef_X2_Max { get; set; }

        public double BothTeamsToScore_Yes_Average { get; set; }
        public double BothTeamsToScore_No_Average { get; set; }
        public double BothTeamsToScore_Yes_Min { get; set; }
        public double BothTeamsToScore_No_Min { get; set; }
        public double BothTeamsToScore_Yes_Max { get; set; }
        public double BothTeamsToScore_No_Max { get; set; }

        public double Total2_5Over_Average { get; set; }
        public double Total2_5Under_Average { get; set; }
        public double Total2_5Over_Min { get; set; }
        public double Total2_5Under_Min { get; set; }
        public double Total2_5Over_Max { get; set; }
        public double Total2_5Under_Max { get; set; }

        //HT = Half-Time
        public double HTHomeWinCoef_Average { get; set; }
        public double HTDrawCoef_Average { get; set; }
        public double HTAwayWinCoef_Average { get; set; }
        public double HTHomeWinCoef_Min { get; set; }
        public double HTDrawCoef_Min { get; set; }
        public double HTAwayWinCoef_Min { get; set; }
        public double HTHomeWinCoef_Max { get; set; }
        public double HTDrawCoef_Max { get; set; }
        public double HTAwayWinCoef_Max { get; set; }
        
        public DateTime LastUpdate { get; set; }

        public void Copy(BookmakerOddsStats oddsStats)
        {
            if (GameId != oddsStats.GameId)
                return;

            HomeWinCoef_Average = oddsStats.HomeWinCoef_Average;
            DrawCoef_Average = oddsStats.DrawCoef_Average;
            AwayWinCoef_Average = oddsStats.AwayWinCoef_Average;
            HomeWinCoef_Min = oddsStats.HomeWinCoef_Min;
            DrawCoef_Min = oddsStats.DrawCoef_Min;
            AwayWinCoef_Min = oddsStats.AwayWinCoef_Min;
            HomeWinCoef_Max = oddsStats.HomeWinCoef_Max;
            DrawCoef_Max = oddsStats.DrawCoef_Max;
            AwayWinCoef_Max = oddsStats.AwayWinCoef_Max;

            DoubleChanceCoef_1X_Average = oddsStats.DoubleChanceCoef_1X_Average;
            DoubleChanceCoef_12_Average = oddsStats.DoubleChanceCoef_12_Average;
            DoubleChanceCoef_X2_Average = oddsStats.DoubleChanceCoef_X2_Average;
            DoubleChanceCoef_1X_Min = oddsStats.DoubleChanceCoef_1X_Min;
            DoubleChanceCoef_12_Min = oddsStats.DoubleChanceCoef_12_Min;
            DoubleChanceCoef_X2_Min = oddsStats.DoubleChanceCoef_X2_Min;
            DoubleChanceCoef_1X_Max = oddsStats.DoubleChanceCoef_1X_Max;
            DoubleChanceCoef_12_Max = oddsStats.DoubleChanceCoef_12_Max;
            DoubleChanceCoef_X2_Max = oddsStats.DoubleChanceCoef_X2_Max;

            BothTeamsToScore_Yes_Average = oddsStats.BothTeamsToScore_Yes_Average;
            BothTeamsToScore_No_Average = oddsStats.BothTeamsToScore_No_Average;
            BothTeamsToScore_Yes_Min = oddsStats.BothTeamsToScore_Yes_Min;
            BothTeamsToScore_No_Min = oddsStats.BothTeamsToScore_No_Min;
            BothTeamsToScore_Yes_Max = oddsStats.BothTeamsToScore_Yes_Max;
            BothTeamsToScore_No_Max = oddsStats.BothTeamsToScore_No_Max;

            Total2_5Over_Average = oddsStats.Total2_5Over_Average;
            Total2_5Under_Average = oddsStats.Total2_5Under_Average;
            Total2_5Over_Min = oddsStats.Total2_5Over_Min;
            Total2_5Under_Min = oddsStats.Total2_5Under_Min;
            Total2_5Over_Max = oddsStats.Total2_5Over_Max;
            Total2_5Under_Max = oddsStats.Total2_5Under_Max;

            HTHomeWinCoef_Average = oddsStats.HTHomeWinCoef_Average;
            HTDrawCoef_Average = oddsStats.HTDrawCoef_Average;
            HTAwayWinCoef_Average = oddsStats.HTAwayWinCoef_Average;
            HTHomeWinCoef_Min = oddsStats.HTHomeWinCoef_Min;
            HTDrawCoef_Min = oddsStats.HTDrawCoef_Min;
            HTAwayWinCoef_Min = oddsStats.HTAwayWinCoef_Min;
            HTHomeWinCoef_Max = oddsStats.HTHomeWinCoef_Max;
            HTDrawCoef_Max = oddsStats.HTDrawCoef_Max;
            HTAwayWinCoef_Max = oddsStats.HTAwayWinCoef_Max;

            LastUpdate = DateTime.Now;
        }

        public virtual Game Game { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FIT.Diploma.Server.Database.LeagueData;

namespace FIT.Diploma.Server.Database.AnalyzerResults
{
    public class Prediction
    {
        [Key]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        public PredictionOption PredictionOption { get; set; }

        public double PredictionOptionCoef { get; set; }

        public double PredictionOptionCoefMax { get; set; }

        public double PredictionOptionCoefMin { get; set; }

        public bool IsFinished { get; set; }

        public bool? IsSucceed { get; set; }

        public void Copy(Prediction prediction)
        {
            if (GameId != prediction.GameId)
                return;
            PredictionOption = prediction.PredictionOption;
            PredictionOptionCoef = prediction.PredictionOptionCoef;
            PredictionOptionCoefMax = prediction.PredictionOptionCoefMax;
            PredictionOptionCoefMin = prediction.PredictionOptionCoefMin;
            IsFinished = prediction.IsFinished;
            IsSucceed = prediction.IsSucceed;
        }

        public virtual Game Game { get; set; }
    }

    public enum PredictionOption
    {
        HomeWin,
        Draw,
        AwayWin,
        TotalOver2_5,
        TotalUnder2_5,
        BttsYes,
        BttsNo
    }
}

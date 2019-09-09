using System;
using System.Collections.Generic;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class PredictionsResponseDto
    {
        public List<PredictionDto> Predictions { get; set; }

        public int LeagueSeasonId { get; set; }
        public int StartYear { get; set; }
        public int LeagueId { get; set; }
    }

    [Serializable]
    public class PredictionDto
    {
        public GameDto Game { get; set; }
        public PredictionOption PredictionOption { get; set; }

        public double PredictionOptionCoef { get; set; }
        public double PredictionOptionCoefMax { get; set; }
        public double PredictionOptionCoefMin { get; set; }

        public bool IsFinished { get; set; }
        public bool? IsSucceed { get; set; }
    }

    [Serializable]
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

namespace FIT.Diploma.Server.DataAnalysis
{
    internal class PredictionData
    {
        public PredictionProbabilities OriginProbabilities { get; set; }
        public PredictionProbabilities CalculatedProbabilities { get; set; }

        public PredictionBookmakerOdds OriginBookmakerOdds { get; set; }
        public PredictionBookmakerOdds CalculatedBookmakerOdds { get; set; }

        internal static PredictionProbabilities CalculateProbabilitiesFromBookmakerOdds(PredictionBookmakerOdds odds)
        {
            PredictionProbabilities result = new PredictionProbabilities();

            if (odds.HomeWinCoef != 0 && odds.DrawCoef != 0 && odds.AwayWinCoef != 0)
            {
                //Probabilities
                result.HomeWinProbability = 100 / odds.HomeWinCoef;
                result.DrawProbability = 100 / odds.DrawCoef;
                result.AwayWinProbability = 100 / odds.AwayWinCoef;
            }

            if (odds.BttsYesCoef != 0 && odds.BttsNoCoef != 0)
            {
                //Probabilities
                result.BttsYesProbability = 100 / odds.BttsYesCoef;
                result.BttsNoProbability = 100 / odds.BttsNoCoef;
            }

            if (odds.TotalOverCoef != 0 && odds.TotalUnderCoef != 0)
            {
                //Probabilities
                result.TotalOverProbability = 100 / odds.TotalOverCoef;
                result.TotalUnderProbability = 100 / odds.TotalUnderCoef;
            }

            return result;
        }

        internal static PredictionBookmakerOdds CalculateBookmakerOddsFromProbabilities(PredictionProbabilities probabilities
                                                ,double mainMargin = 0, double bttsMargin = 0, double totalMargin = 0)
        {
            PredictionBookmakerOdds result = new PredictionBookmakerOdds();

            if (probabilities.HomeWinProbability != 0 && probabilities.DrawProbability != 0 && probabilities.AwayWinProbability != 0 && mainMargin != -100)
            {
                if(mainMargin == 0)
                    mainMargin =  probabilities.GetMainMargin() ;

                var allProbablities = probabilities.HomeWinProbability + probabilities.DrawProbability + probabilities.AwayWinProbability;
                var temp = allProbablities * 100 / (100 + mainMargin);

                result.HomeWinCoef = temp / probabilities.HomeWinProbability;
                result.DrawCoef = temp / probabilities.DrawProbability;
                result.AwayWinCoef = temp / probabilities.AwayWinProbability;
            }

            if (probabilities.BttsYesProbability != 0 && probabilities.BttsNoProbability != 0 && bttsMargin != -100)
            {
                if (bttsMargin == 0) bttsMargin = probabilities.GetBttsMargin() / 2;
                var allProbablities = probabilities.BttsYesProbability + probabilities.BttsNoProbability;
                var temp = allProbablities * 100 / (100 + bttsMargin);

                result.BttsYesCoef = temp / probabilities.BttsYesProbability;
                result.BttsNoCoef = temp / probabilities.BttsNoProbability;
            }

            if (probabilities.TotalOverProbability != 0 && probabilities.TotalUnderProbability != 0 && totalMargin != -100)
            {
                if (totalMargin == 0) totalMargin = probabilities.GetTotalMargin() / 2;
                var allProbablities = probabilities.TotalOverProbability + probabilities.TotalUnderProbability;
                var temp = allProbablities * 100 / (100 + totalMargin);

                result.TotalOverCoef = temp / probabilities.TotalOverProbability;
                result.TotalUnderCoef = temp / probabilities.TotalUnderProbability;
            }

            return result;
        }
    }

    internal class PredictionProbabilities
    {
        public PredictionProbabilities() { }

        public PredictionProbabilities(PredictionProbabilities copy)
        {
            HomeWinProbability = copy.HomeWinProbability;
            DrawProbability = copy.DrawProbability;
            AwayWinProbability = copy.AwayWinProbability;

            BttsYesProbability = copy.BttsYesProbability;
            BttsNoProbability = copy.BttsNoProbability;

            TotalOverProbability = copy.TotalOverProbability;
            TotalUnderProbability = copy.TotalUnderProbability;
        }

        //Probabilities
        public double HomeWinProbability { get; set; }
        public double DrawProbability { get; set; }
        public double AwayWinProbability { get; set; }

        public double GetMainMargin()
        {
            return HomeWinProbability + DrawProbability + AwayWinProbability - 100;
        }

        public double BttsYesProbability { get; set; }
        public double BttsNoProbability { get; set; }

        public double GetBttsMargin()
        {
            return BttsYesProbability + BttsNoProbability - 100;
        }

        public double TotalOverProbability { get; set; }
        public double TotalUnderProbability { get; set; }

        public double GetTotalMargin()
        {
            return TotalOverProbability + TotalUnderProbability - 100;
        }
    }

    internal class PredictionBookmakerOdds
    {
        public PredictionBookmakerOdds() { }

        public PredictionBookmakerOdds(PredictionBookmakerOdds copy)
        {
            HomeWinCoef = copy.HomeWinCoef;
            DrawCoef = copy.DrawCoef;
            AwayWinCoef = copy.AwayWinCoef;

            BttsYesCoef = copy.BttsYesCoef;
            BttsNoCoef = copy.BttsNoCoef;

            TotalOverCoef = copy.TotalOverCoef;
            TotalUnderCoef = copy.TotalUnderCoef;
        }

        //BookmakerOdds
        public double HomeWinCoef { get; set; }
        public double DrawCoef { get; set; }
        public double AwayWinCoef { get; set; }

        public double BttsYesCoef { get; set; }
        public double BttsNoCoef { get; set; }

        public double TotalOverCoef { get; set; }
        public double TotalUnderCoef { get; set; }
    }
}

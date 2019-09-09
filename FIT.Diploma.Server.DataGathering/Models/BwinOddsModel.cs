using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataGathering.Models
{
    public class BwinOddsModel
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }

        public double HomeWinCoef { get; set; }
        public double DrawCoef { get; set; }
        public double AwayWinCoef { get; set; }

        public double DoubleChanceCoef_1X { get; set; }
        public double DoubleChanceCoef_12 { get; set; }
        public double DoubleChanceCoef_X2 { get; set; }

        public double BothTeamsToScore_Yes { get; set; }
        public double BothTeamsToScore_No { get; set; }

        public double Total2_5Over { get; set; }
        public double Total2_5Under { get; set; }

        //HT = Half-Time
        public double HTHomeWinCoef { get; set; }
        public double HTDrawCoef { get; set; }
        public double HTAwayWinCoef { get; set; }
    }
}

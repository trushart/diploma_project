using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataGathering.Models
{
    public class FootballDataModel
    {
        #region Main Stats

        public string Division { get; set; }
        public DateTime MatchDate { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }

        public int FullTimeHomeTeamGoals { get; set; }
        public int FullTimeAwayTeamGoals { get; set; }
        //(H=Home Win, D=Draw, A=Away Win)
        public string FullTimeResult { get; set; }

        public int HalfTimeHomeTeamGoals { get; set; }
        public int HalfTimeAwayTeamGoals { get; set; }
        public string HalfTimeResult { get; set; }

        #endregion Main Stats

        #region Additional Stats

        public string Attendance { get; set; }
        public string Referee { get; set; }

        public int HomeTeamShots { get; set; }
        public int AwayTeamShots { get; set; }
        public int HomeTeamShotsOnTarget { get; set; }
        public int AwayTeamShotsOnTarget { get; set; }
        public int HomeTeamHitWoodwork { get; set; }
        public int AwayTeamHitWoodwork { get; set; }

        public int HomeTeamCorners { get; set; }
        public int AwayTeamCorners { get; set; }

        public int HomeTeamFouls { get; set; }
        public int AwayTeamFouls { get; set; }

        public int HomeTeamOffsides { get; set; }
        public int AwayTeamOffsides { get; set; }

        public int HomeTeamYellowCards { get; set; }
        public int AwayTeamYellowCards { get; set; }

        public int HomeTeamRedCards { get; set; }
        public int AwayTeamRedCards { get; set; }

        //points (10 = yellow, 25 = red)
        public int HomeTeamBookingPoints { get; set; }
        public int AwayTeamBookingPoints { get; set; }

        #endregion Additional Stats

        #region Betting Odds

        //Bet365
        public double B365_HomeWin { get; set; }
        public double B365_Draw { get; set; }
        public double B365_AwayWin { get; set; }

        public double B365_TotalMore2_5 { get; set; }
        public double B365_TotalLess2_5 { get; set; }

        //Blue Square
        public double BS_HomeWin { get; set; }
        public double BS_Draw { get; set; }
        public double BS_AwayWin { get; set; }

        //Bet&Win
        public double BW_HomeWin { get; set; }
        public double BW_Draw { get; set; }
        public double BW_AwayWin { get; set; }

        //Gamebookers 
        public double GB_HomeWin { get; set; }
        public double GB_Draw { get; set; }
        public double GB_AwayWin { get; set; }

        public double GB_TotalMore2_5 { get; set; }
        public double GB_TotalLess2_5 { get; set; }

        //Interwetten 
        public double IW_HomeWin { get; set; }
        public double IW_Draw { get; set; }
        public double IW_AwayWin { get; set; }

        //Ladbrokes 
        public double LB_HomeWin { get; set; }
        public double LB_Draw { get; set; }
        public double LB_AwayWin { get; set; }

        //Pinnacle 
        public double Pinnacle_HomeWin { get; set; }
        public double Pinnacle_Draw { get; set; }
        public double Pinnacle_AwayWin { get; set; }
        public double PinnacleClose_HomeWin { get; set; }
        public double PinnacleClose_Draw { get; set; }
        public double PinnacleClose_AwayWin { get; set; }

        //Sporting Odds
        public double SO_HomeWin { get; set; }
        public double SO_Draw { get; set; }
        public double SO_AwayWin { get; set; }

        //Sportingbet 
        public double SB_HomeWin { get; set; }
        public double SB_Draw { get; set; }
        public double SB_AwayWin { get; set; }

        //Stan James
        public double SJ_HomeWin { get; set; }
        public double SJ_Draw { get; set; }
        public double SJ_AwayWin { get; set; }

        //Stanleybet
        public double Stanleybet_HomeWin { get; set; }
        public double Stanleybet_Draw { get; set; }
        public double Stanleybet_AwayWin { get; set; }

        //VC
        public double VC_HomeWin { get; set; }
        public double VC_Draw { get; set; }
        public double VC_AwayWin { get; set; }

        //William Hill
        public double WH_HomeWin { get; set; }
        public double WH_Draw { get; set; }
        public double WH_AwayWin { get; set; }

        //Total average odds
        public int NumberOfAllBookmakers { get; set; }
        public double Max_TotalMore2_5 { get; set; }
        public double Max_TotalLess2_5 { get; set; }

        public double Average_TotalMore2_5 { get; set; }
        public double Average_TotalLess2_5 { get; set; }

        #endregion Betting Odds

        //parse date (format = dd/mm/yy)
        private DateTime ParseDateFromString(string value, out bool result)
        {
            string[] dateParts = value.Split('/');
            result = true;
            if(dateParts.Count() != 3)
            {
                result = false;
                //to do: log error
                return new DateTime();
            }
            int day, month, year;
            result = Int32.TryParse(dateParts[0], out day);
            if (!result)
            {
                //to do: log error
                return new DateTime();
            }
            result = Int32.TryParse(dateParts[1], out month);
            if (!result)
            {
                //to do: log error
                return new DateTime();
            }
            result = Int32.TryParse(dateParts[2], out year);
            if (!result)
            {
                //to do: log error
                return new DateTime();
            }
            if(year < 1950)
            {
                if (year <= 18) year += 2000;
                else year += 1900;
            }            

            return new DateTime(year, month, day);
        }

        public bool SetProperty(string property, string value)
        {
            bool parseResult = true;
            int parseResultInt;
            double parseResultDouble;
            switch (property)
            {
                case "Div":
                    Division = value;
                    break;
                case "Date":                    
                    MatchDate = ParseDateFromString(value, out parseResult);
                    break;
                case "HomeTeam":
                    HomeTeam = value;
                    break;
                case "AwayTeam":
                    AwayTeam = value;
                    break;
                case "HG":
                case "FTHG":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) FullTimeHomeTeamGoals = parseResultInt;
                    break;
                case "AG":
                case "FTAG":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) FullTimeAwayTeamGoals = parseResultInt;
                    break;
                case "Res":
                case "FTR":
                    FullTimeResult = value;
                    break;
                case "HTHG":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HalfTimeHomeTeamGoals = parseResultInt;
                    break;
                case "HTAG":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HalfTimeAwayTeamGoals = parseResultInt;
                    break;
                case "HTR":
                    HalfTimeResult = value;
                    break;
                case "Attendance":
                    Attendance = value;
                    break;
                case "Referee":
                    Referee = value;
                    break;
                case "HS":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HomeTeamShots = parseResultInt;
                    break;
                case "AS":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) AwayTeamShots = parseResultInt;
                    break;
                case "HST":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HomeTeamShotsOnTarget = parseResultInt;
                    break;
                case "AST":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) AwayTeamShotsOnTarget = parseResultInt;
                    break;
                case "HHW":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HomeTeamHitWoodwork = parseResultInt;
                    break;
                case "AHW":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) AwayTeamHitWoodwork = parseResultInt;
                    break;
                case "HC":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HomeTeamCorners = parseResultInt;
                    break;
                case "AC":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) AwayTeamCorners = parseResultInt;
                    break;
                case "HF":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HomeTeamFouls = parseResultInt;
                    break;
                case "AF":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) AwayTeamFouls = parseResultInt;
                    break;
                case "HO":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HomeTeamOffsides = parseResultInt;
                    break;
                case "AO":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) AwayTeamOffsides = parseResultInt;
                    break;
                case "HY":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HomeTeamYellowCards = parseResultInt;
                    break;
                case "AY":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) AwayTeamYellowCards = parseResultInt;
                    break;
                case "HR":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HomeTeamRedCards = parseResultInt;
                    break;
                case "AR":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) AwayTeamRedCards = parseResultInt;
                    break;
                case "HBP":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) HomeTeamBookingPoints = parseResultInt;
                    break;
                case "ABP":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) AwayTeamBookingPoints = parseResultInt;
                    break;
                case "B365H":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) B365_HomeWin = parseResultDouble;
                    break;
                case "B365D":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) B365_Draw = parseResultDouble;
                    break;
                case "B365A":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) B365_AwayWin = parseResultDouble;
                    break;
                case "BSH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) BS_HomeWin = parseResultDouble;
                    break;
                case "BSD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) BS_Draw = parseResultDouble;
                    break;
                case "BSA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) BS_AwayWin = parseResultDouble;
                    break;
                case "BWH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) BW_HomeWin = parseResultDouble;
                    break;
                case "BWD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) BW_Draw = parseResultDouble;
                    break;
                case "BWA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) BW_AwayWin = parseResultDouble;
                    break;
                case "GBH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) GB_HomeWin = parseResultDouble;
                    break;
                case "GBD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) GB_Draw = parseResultDouble;
                    break;
                case "GBA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) GB_AwayWin = parseResultDouble;
                    break;
                case "IWH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) IW_HomeWin = parseResultDouble;
                    break;
                case "IWD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) IW_Draw = parseResultDouble;
                    break;
                case "IWA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) IW_AwayWin = parseResultDouble;
                    break;
                case "LBH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) LB_HomeWin = parseResultDouble;
                    break;
                case "LBD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) LB_Draw = parseResultDouble;
                    break;
                case "LBA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) LB_AwayWin = parseResultDouble;
                    break;
                case "PH":
                case "PSH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) Pinnacle_HomeWin = parseResultDouble;
                    break;
                case "PD":
                case "PSD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) Pinnacle_Draw = parseResultDouble;
                    break;
                case "PA":
                case "PSA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) Pinnacle_AwayWin = parseResultDouble;
                    break;
                case "SOH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) SO_HomeWin = parseResultDouble;
                    break;
                case "SOD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) SO_Draw = parseResultDouble;
                    break;
                case "SOA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) SO_AwayWin = parseResultDouble;
                    break;
                case "SBH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) SB_HomeWin = parseResultDouble;
                    break;
                case "SBD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) SB_Draw = parseResultDouble;
                    break;
                case "SBA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) SB_AwayWin = parseResultDouble;
                    break;
                case "SJH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) SJ_HomeWin = parseResultDouble;
                    break;
                case "SJD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) SJ_Draw = parseResultDouble;
                    break;
                case "SJA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) SJ_AwayWin = parseResultDouble;
                    break;
                case "SYH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) Stanleybet_HomeWin = parseResultDouble;
                    break;
                case "SYD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) Stanleybet_Draw = parseResultDouble;
                    break;
                case "SYA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) Stanleybet_AwayWin = parseResultDouble;
                    break;
                case "VCH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) VC_HomeWin = parseResultDouble;
                    break;
                case "VCD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) VC_Draw = parseResultDouble;
                    break;
                case "VCA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) VC_AwayWin = parseResultDouble;
                    break;
                case "WHH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) WH_HomeWin = parseResultDouble;
                    break;
                case "WHD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) WH_Draw = parseResultDouble;
                    break;
                case "WHA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) WH_AwayWin = parseResultDouble;
                    break;
                case "PSCH":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) PinnacleClose_HomeWin = parseResultDouble;
                    break;
                case "PSCD":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) PinnacleClose_Draw = parseResultDouble;
                    break;
                case "PSCA":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) PinnacleClose_AwayWin = parseResultDouble;
                    break;
                case "BbOU":
                    parseResult = Int32.TryParse(value, out parseResultInt);
                    if (parseResult) NumberOfAllBookmakers = parseResultInt;
                    break;
                case "BbMx>2.5":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) Max_TotalMore2_5 = parseResultDouble;
                    break;
                case "BbAv>2.5":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) Average_TotalMore2_5 = parseResultDouble;
                    break;
                case "BbMx<2.5":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) Max_TotalLess2_5 = parseResultDouble;
                    break;
                case "BbAv<2.5":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) Average_TotalLess2_5 = parseResultDouble;
                    break;
                case "GB>2.5":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) GB_TotalMore2_5 = parseResultDouble;
                    break;
                case "GB<2.5":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) GB_TotalLess2_5 = parseResultDouble;
                    break;
                case "B365>2.5":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) B365_TotalMore2_5 = parseResultDouble;
                    break;
                case "B365<2.5":
                    parseResult = Double.TryParse(value, out parseResultDouble);
                    if (parseResult) B365_TotalLess2_5 = parseResultDouble;
                    break;
                case "Bb1X2":
                case "BbMxH":
                case "BbAvH":
                case "BbMxD":
                case "BbAvD":
                case "BbMxA":
                case "BbAvA":
                case "MaxH":
                case "MaxD":
                case "MaxA":
                case "AvgH":
                case "AvgD":
                case "AvgA":
                case "BbAH":
                case "BbAHh":
                case "BbMxAHH":
                case "BbAvAHH":
                case "BbMxAHA":
                case "BbAvAHA":
                case "GBAHH":
                case "GBAHA":
                case "GBAH":
                case "LBAHH":
                case "LBAHA":
                case "LBAH":
                case "B365AHH":
                case "B365AHA":
                case "B365AH":
                    //available data that for now not required (could be used in future)
                    break;
                default:
                    return false;
            }
            return parseResult;
        }
    }
}

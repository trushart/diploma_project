using FIT.Diploma.Server.Database.AnalyzerResults;
using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FIT.Diploma.Server.DataAnalysis
{
    internal class PredictionHelper
    {
        //consts
        private static double StreakBreakerCoef = 1.04;
        private static double SeasonStatsCoef = 1.01;

        private static double TeamFormCoef = 1.03;

        private static double H2HCoef = 1.03;
        private static double H2HCoefLastGames = 1.1;
        
        private static double RoundStatsCoef = 1.03;
        private static double RoundStatsCoefLastRounds = 1.06;

        public static PredictionOption MakeDecision(PredictionData data , out double maxDiffProbability, out double resultProbability, out double resultCoef)
        {
            PredictionOption maxDiffProbabilityOption = 0;
            maxDiffProbability = -1;
            resultProbability = -1;
            resultCoef = -1;

            //main results
            var tempDiff = data.CalculatedProbabilities.HomeWinProbability - data.OriginProbabilities.HomeWinProbability;
            if (tempDiff > maxDiffProbability)
            {
                maxDiffProbability = tempDiff;
                maxDiffProbabilityOption = PredictionOption.HomeWin;
                resultProbability = data.CalculatedProbabilities.HomeWinProbability;
                resultCoef = data.OriginBookmakerOdds.HomeWinCoef;
            }

            tempDiff = data.CalculatedProbabilities.DrawProbability - data.OriginProbabilities.DrawProbability;
            if (tempDiff > maxDiffProbability)
            {
                maxDiffProbability = tempDiff;
                maxDiffProbabilityOption = PredictionOption.Draw;
                resultProbability = data.CalculatedProbabilities.DrawProbability;
                resultCoef = data.OriginBookmakerOdds.DrawCoef;
            }

            tempDiff = data.CalculatedProbabilities.AwayWinProbability - data.OriginProbabilities.AwayWinProbability;
            if (tempDiff > maxDiffProbability)
            {
                maxDiffProbability = tempDiff;
                maxDiffProbabilityOption = PredictionOption.AwayWin;
                resultProbability = data.CalculatedProbabilities.AwayWinProbability;
                resultCoef = data.OriginBookmakerOdds.AwayWinCoef;
            }

            //btts
            tempDiff = data.CalculatedProbabilities.BttsYesProbability - data.OriginProbabilities.BttsYesProbability;
            if (tempDiff > maxDiffProbability)
            {
                maxDiffProbability = tempDiff;
                maxDiffProbabilityOption = PredictionOption.BttsYes;
                resultProbability = data.CalculatedProbabilities.BttsYesProbability;
                resultCoef = data.OriginBookmakerOdds.BttsYesCoef;
            }

            tempDiff = data.CalculatedProbabilities.BttsNoProbability - data.OriginProbabilities.BttsNoProbability;
            if (tempDiff > maxDiffProbability)
            {
                maxDiffProbability = tempDiff;
                maxDiffProbabilityOption = PredictionOption.BttsNo;
                resultProbability = data.CalculatedProbabilities.BttsNoProbability;
                resultCoef = data.OriginBookmakerOdds.BttsNoCoef;
            }

            //total
            tempDiff = data.CalculatedProbabilities.TotalOverProbability - data.OriginProbabilities.TotalOverProbability;
            if (tempDiff > maxDiffProbability)
            {
                maxDiffProbability = tempDiff;
                maxDiffProbabilityOption = PredictionOption.TotalOver2_5;
                resultProbability = data.CalculatedProbabilities.TotalOverProbability;
                resultCoef = data.OriginBookmakerOdds.TotalOverCoef;
            }

            tempDiff = data.CalculatedProbabilities.TotalUnderProbability - data.OriginProbabilities.TotalUnderProbability;
            if (tempDiff > maxDiffProbability)
            {
                maxDiffProbability = tempDiff;
                maxDiffProbabilityOption = PredictionOption.TotalUnder2_5;
                resultProbability = data.CalculatedProbabilities.TotalUnderProbability;
                resultCoef = data.OriginBookmakerOdds.TotalUnderCoef;
            }

            return maxDiffProbabilityOption;
        }

        #region Rounds

        public static PredictionOption GetOptionWithMaxPropability(PredictionData data, out double resultProbability, out double resultCoef)
        {
            PredictionOption result = 0;
            resultProbability = 0;
            resultCoef = 0;

            //main results
            if (data.OriginProbabilities.HomeWinProbability > resultProbability)
            {
                result = PredictionOption.HomeWin;
                resultProbability = data.OriginProbabilities.HomeWinProbability;
                resultCoef = data.OriginBookmakerOdds.HomeWinCoef;
            }

            if (data.OriginProbabilities.DrawProbability > resultProbability)
            {
                result = PredictionOption.Draw;
                resultProbability = data.OriginProbabilities.DrawProbability;
                resultCoef = data.OriginBookmakerOdds.DrawCoef;
            }

            if (data.OriginProbabilities.AwayWinProbability > resultProbability)
            {
                result = PredictionOption.AwayWin;
                resultProbability = data.OriginProbabilities.AwayWinProbability;
                resultCoef = data.OriginBookmakerOdds.AwayWinCoef;
            }

            //btts
            if (data.OriginProbabilities.BttsYesProbability > resultProbability)
            {
                result = PredictionOption.BttsYes;
                resultProbability = data.OriginProbabilities.BttsYesProbability;
                resultCoef = data.OriginBookmakerOdds.BttsYesCoef;
            }

            if (data.OriginProbabilities.BttsNoProbability > resultProbability)
            {
                result = PredictionOption.BttsNo;
                resultProbability = data.OriginProbabilities.BttsNoProbability;
                resultCoef = data.OriginBookmakerOdds.BttsNoCoef;
            }

            //total
            if (data.OriginProbabilities.TotalOverProbability > resultProbability)
            {
                result = PredictionOption.TotalOver2_5;
                resultProbability = data.OriginProbabilities.TotalOverProbability;
                resultCoef = data.OriginBookmakerOdds.TotalOverCoef;
            }

            if (data.OriginProbabilities.TotalUnderProbability > resultProbability)
            {
                result = PredictionOption.TotalUnder2_5;
                resultProbability = data.OriginProbabilities.TotalUnderProbability;
                resultCoef = data.OriginBookmakerOdds.TotalUnderCoef;
            }

            return result;
        }

        
        public static void CheckRoundStats(PredictionData data, Game game)
        {
            var leagueRepo = new LeagueDataRepository();
            var analysisRepo = new AnalysisDataRepository();
            var allSeasons = leagueRepo.GetAllLeagueSeasons(leagueRepo.GetLaLiga()).OrderByDescending(s => s.StartYear).ToList();

            int currentSeasonId = allSeasons.First().LeagueSeasonId;
            var currentSeasonRounds = leagueRepo.GetCurrentSeasonRounds(currentSeasonId);
            currentSeasonRounds = currentSeasonRounds.Where(r => r.RoundNumber <= game.SeasonRound.RoundNumber && CheckRound(r))
                                                    .OrderByDescending(r => r.RoundNumber).Take(5).ToList();

            var currentSeasonRoundsStats = analysisRepo.GetAverageRoundStats(currentSeasonId);
            var previousSeasonRoundsStats = allSeasons.Count > 1 ? analysisRepo.GetAverageRoundStats(allSeasons[1].LeagueSeasonId) : null;
            var previous2SeasonRoundsStats = allSeasons.Count > 2 ? analysisRepo.GetAverageRoundStats(allSeasons[2].LeagueSeasonId) : null;

            //average
            var averageHomeWins = (previous2SeasonRoundsStats.HomeWinsCount_Average + previousSeasonRoundsStats.HomeWinsCount_Average) / 2;
            var averagDraws = (previous2SeasonRoundsStats.DrawsCount_Average + previousSeasonRoundsStats.DrawsCount_Average) / 2;
            var averageAwayWins = (previous2SeasonRoundsStats.AwayWinsCount_Average + previousSeasonRoundsStats.AwayWinsCount_Average) / 2;

            var averageBttsYes = (previous2SeasonRoundsStats.BTTS_Yes_Average + previousSeasonRoundsStats.BTTS_Yes_Average) / 2;
            var averageBttsNo= (previous2SeasonRoundsStats.BTTS_No_Average + previousSeasonRoundsStats.BTTS_No_Average) / 2;

            var averageTotalOver = (previous2SeasonRoundsStats.GamesOver2_5_Average + previousSeasonRoundsStats.GamesOver2_5_Average) / 2;
            var averagTotalUnder = (previous2SeasonRoundsStats.GamesUnder2_5_Average + previousSeasonRoundsStats.GamesUnder2_5_Average) / 2;

            //get maximal diff compared current season with last seasons
            var currentDiffHomeWins = averageHomeWins - currentSeasonRoundsStats.HomeWinsCount_Average;
            var currentDiffDraws = averagDraws - currentSeasonRoundsStats.DrawsCount_Average;
            var currentDiffAwayWins = averageAwayWins - currentSeasonRoundsStats.AwayWinsCount_Average;

            var currentDiffBttsYes = averageBttsYes - currentSeasonRoundsStats.BTTS_Yes_Average;
            var currentDiffBttsNo = averageBttsNo - currentSeasonRoundsStats.BTTS_No_Average;

            var currentDiffTotalOver = averageTotalOver - currentSeasonRoundsStats.GamesOver2_5_Average;
            var currentDiffTotalUnder = averagTotalUnder - currentSeasonRoundsStats.GamesUnder2_5_Average;

            var maxDiff = currentDiffHomeWins;
            if (currentDiffDraws > maxDiff) maxDiff = currentDiffDraws;
            if (currentDiffAwayWins > maxDiff) maxDiff = currentDiffAwayWins;

            if (maxDiff == currentDiffHomeWins)
                data.CalculatedProbabilities.HomeWinProbability *= RoundStatsCoef;
            if (maxDiff == currentDiffDraws)
                data.CalculatedProbabilities.DrawProbability *= RoundStatsCoef;
            if (maxDiff == currentDiffAwayWins)
                data.CalculatedProbabilities.AwayWinProbability *= RoundStatsCoef;

            maxDiff = currentDiffBttsYes;
            if (currentDiffBttsNo > maxDiff) maxDiff = currentDiffBttsNo;

            if (maxDiff == currentDiffBttsYes)
                data.CalculatedProbabilities.BttsYesProbability *= RoundStatsCoef;
            if (maxDiff == currentDiffBttsNo)
                data.CalculatedProbabilities.BttsNoProbability *= RoundStatsCoef;

            maxDiff = currentDiffTotalOver;
            if (currentDiffTotalUnder > maxDiff) maxDiff = currentDiffTotalUnder;

            if (maxDiff == currentDiffTotalOver)
                data.CalculatedProbabilities.TotalOverProbability *= RoundStatsCoef;
            if (maxDiff == currentDiffTotalUnder)
                data.CalculatedProbabilities.TotalUnderProbability *= RoundStatsCoef;



            //get maximal diff compared last 5 rounds with last seasons
            currentDiffHomeWins = 0;
            currentDiffDraws = 0;
            currentDiffAwayWins = 0;
            currentDiffBttsYes = 0;
            currentDiffBttsNo = 0;
            currentDiffTotalOver = 0;
            currentDiffTotalUnder = 0;

            int i = 1;
            foreach (var round in currentSeasonRounds)
            {
                currentDiffHomeWins += (averageHomeWins - round.HomeWinsCount) / i;
                currentDiffDraws += (averagDraws - round.DrawsCount) / i;
                currentDiffAwayWins += (averageAwayWins - round.AwayWinsCount) / i;

                currentDiffBttsYes += (averageBttsYes - round.BTTS_Yes) / i;
                currentDiffBttsNo += (averageBttsNo - round.BTTS_No) / i;

                currentDiffTotalOver += (averageTotalOver - round.GamesOver2_5) / i;
                currentDiffTotalUnder += (averagTotalUnder - round.GamesUnder2_5) / i;
                i++;
            }

            maxDiff = currentDiffHomeWins;
            if (currentDiffDraws > maxDiff) maxDiff = currentDiffDraws;
            if (currentDiffAwayWins > maxDiff) maxDiff = currentDiffAwayWins;

            if (maxDiff == currentDiffHomeWins)
                data.CalculatedProbabilities.HomeWinProbability *= RoundStatsCoefLastRounds;
            if (maxDiff == currentDiffDraws)
                data.CalculatedProbabilities.DrawProbability *= RoundStatsCoefLastRounds;
            if (maxDiff == currentDiffAwayWins)
                data.CalculatedProbabilities.AwayWinProbability *= RoundStatsCoefLastRounds;

            maxDiff = currentDiffBttsYes;
            if (currentDiffBttsNo > maxDiff) maxDiff = currentDiffBttsNo;

            if (maxDiff == currentDiffBttsYes)
                data.CalculatedProbabilities.BttsYesProbability *= RoundStatsCoefLastRounds;
            if (maxDiff == currentDiffBttsNo)
                data.CalculatedProbabilities.BttsNoProbability *= RoundStatsCoefLastRounds;

            maxDiff = currentDiffTotalOver;
            if (currentDiffTotalUnder > maxDiff) maxDiff = currentDiffTotalUnder;

            if (maxDiff == currentDiffTotalOver)
                data.CalculatedProbabilities.TotalOverProbability *= RoundStatsCoefLastRounds;
            if (maxDiff == currentDiffTotalUnder)
                data.CalculatedProbabilities.TotalUnderProbability *= RoundStatsCoefLastRounds;
        }

        private static bool CheckRound(SeasonRound round)
        {
            return round.GamePlayed == round.AwayWinsCount + round.HomeWinsCount + round.DrawsCount;
        }
        #endregion Rounds 

        #region H2H
        public static void CheckH2H(PredictionData data, Game game)
        {
            var leagueRepo = new LeagueDataRepository();
            var analysisRepo = new AnalysisDataRepository();

            var homeTeamId = game.HomeTeamId;
            var awayTeamId = game.AwayTeamId;
            var h2hStats = analysisRepo.GetTeamsHeadToHead(homeTeamId, awayTeamId);
            var teamsHistory = leagueRepo.GetTeamsHistory(homeTeamId, awayTeamId).Where(g => g.Result != 0)
                                    .OrderByDescending(th => th.Date).ToList();

            bool homeTeamMapsToTeam1 = h2hStats.Team1Id == homeTeamId;

            int lastHomeTeamWinGamesAgo = -1, lastAwayTeamWinGamesAgo = -1, lastDrawGamesAgo = -1;
            int lastBttsYes = -1, lastBttsNo = -1;
            int lastTotalOver = -1, lastTotalUnder = -1;
            for (int i = 0; i < teamsHistory.Count; i++)
            {
                if (lastHomeTeamWinGamesAgo > -1 && lastAwayTeamWinGamesAgo > -1 && lastDrawGamesAgo > -1
                    && lastTotalOver > -1 && lastTotalUnder > -1  && lastBttsYes > -1 && lastBttsNo > -1)
                    break;
                var tempGame = teamsHistory[i];
                if(tempGame.HomeTeamId == homeTeamId)
                {
                    switch (tempGame.Result)
                    {
                        case GameResult.HomeWin:
                            if (lastHomeTeamWinGamesAgo == -1)
                                lastHomeTeamWinGamesAgo = i;
                            break;
                        case GameResult.Draw:
                            if (lastDrawGamesAgo == -1)
                                lastDrawGamesAgo = i;
                            break;
                        case GameResult.AwayWin:
                            if (lastAwayTeamWinGamesAgo == -1)
                                lastAwayTeamWinGamesAgo = i;
                            break;
                    }
                }
                else
                {
                    switch (tempGame.Result)
                    {
                        case GameResult.HomeWin:
                            if (lastAwayTeamWinGamesAgo == -1)
                                lastAwayTeamWinGamesAgo = i;                            
                            break;
                        case GameResult.Draw:
                            if (lastDrawGamesAgo == -1)
                                lastDrawGamesAgo = i;
                            break;
                        case GameResult.AwayWin:
                            if (lastHomeTeamWinGamesAgo == -1)
                                lastHomeTeamWinGamesAgo = i;
                            break;
                    }
                }

                var totalOver = (tempGame.AwayTeamGoals + tempGame.HomeTeamGoals) > 2;
                if (totalOver && (lastTotalOver == -1)) lastTotalOver = i;
                if (!totalOver && (lastTotalUnder == -1)) lastTotalUnder = i;

                var bttsYes = (tempGame.AwayTeamGoals > 0) && (tempGame.HomeTeamGoals > 0);
                if (bttsYes && (lastBttsYes == -1)) lastBttsYes = i;
                if (!bttsYes && (lastBttsNo == -1)) lastBttsNo = i;
            }

            double averageHomeTeamWinFreq, averageAwayTeamWinFreq, averageDrawsFreq;
            double averageBttsYesFreq, averageBttsNoFreq;
            double averageTotalOverFreq, averageTotalUnderFreq;
            if (homeTeamMapsToTeam1)
            {
                averageHomeTeamWinFreq = 1 / h2hStats.Team1WinsPercentage;
                averageAwayTeamWinFreq = 1 / h2hStats.Team2WinsPercentage;
            }
            else
            {
                averageAwayTeamWinFreq = 1 / h2hStats.Team1WinsPercentage;
                averageHomeTeamWinFreq = 1 / h2hStats.Team2WinsPercentage;
            }
            averageDrawsFreq = 1 / h2hStats.DrawsPercentage;

            averageBttsYesFreq = (double)h2hStats.GamePlayed / h2hStats.BTTS_Yes;
            averageBttsNoFreq = (double)h2hStats.GamePlayed / h2hStats.BTTS_No;

            averageTotalOverFreq = (double)h2hStats.GamePlayed / h2hStats.GamesOver2_5;
            averageTotalUnderFreq = (double)h2hStats.GamePlayed / h2hStats.GamesUnder2_5;

            var minFreq = averageHomeTeamWinFreq;
            if (averageAwayTeamWinFreq < minFreq) minFreq = averageAwayTeamWinFreq;
            if (averageDrawsFreq < minFreq) minFreq = averageDrawsFreq;

            if (minFreq == averageHomeTeamWinFreq)
                data.CalculatedProbabilities.HomeWinProbability *= H2HCoef;
            if (minFreq == averageDrawsFreq)
                data.CalculatedProbabilities.DrawProbability *= H2HCoef;
            if (minFreq == averageAwayTeamWinFreq)
                data.CalculatedProbabilities.AwayWinProbability *= H2HCoef;

            minFreq = averageBttsYesFreq;
            if (averageBttsNoFreq < minFreq) minFreq = averageBttsNoFreq;

            if (minFreq == averageBttsNoFreq)
                data.CalculatedProbabilities.BttsNoProbability *= H2HCoef;
            if (minFreq == averageBttsYesFreq)
                data.CalculatedProbabilities.BttsYesProbability *= H2HCoef;

            minFreq = averageTotalOverFreq;
            if (averageTotalUnderFreq < minFreq) minFreq = averageTotalUnderFreq;

            if (minFreq == averageTotalOverFreq)
                data.CalculatedProbabilities.TotalOverProbability *= H2HCoef;
            if (minFreq == averageTotalUnderFreq)
                data.CalculatedProbabilities.TotalUnderProbability *= H2HCoef;

            //differences between last time some result happend and average result frequency 
            var diffHomeTeamWins = lastHomeTeamWinGamesAgo - averageHomeTeamWinFreq;
            var diffAwayTeamWins = lastAwayTeamWinGamesAgo - averageAwayTeamWinFreq;
            var diffDraws = lastDrawGamesAgo - averageDrawsFreq;

            var diffBttsYes= lastBttsYes - averageBttsYesFreq;
            var diffBttsNo = lastBttsNo - averageBttsNoFreq;

            var diffTotalOver= lastTotalOver - averageTotalOverFreq;
            var diffTotalUnder = lastTotalUnder - averageTotalUnderFreq;

            var maxDiff = diffHomeTeamWins;
            if (diffAwayTeamWins > maxDiff) maxDiff = diffAwayTeamWins;
            if (diffDraws > maxDiff) maxDiff = diffDraws;

            if (maxDiff == diffHomeTeamWins)
                data.CalculatedProbabilities.HomeWinProbability *= H2HCoefLastGames;
            if (maxDiff == diffDraws)
                data.CalculatedProbabilities.DrawProbability *= H2HCoefLastGames;
            if (maxDiff == diffAwayTeamWins)
                data.CalculatedProbabilities.AwayWinProbability *= H2HCoefLastGames;

            maxDiff = diffBttsYes;
            if (diffBttsNo > maxDiff) maxDiff = diffBttsNo;

            if (maxDiff == diffBttsYes)
                data.CalculatedProbabilities.BttsYesProbability *= H2HCoefLastGames;
            if (maxDiff == diffBttsNo)
                data.CalculatedProbabilities.BttsNoProbability *= H2HCoefLastGames;

            maxDiff = diffTotalOver;
            if (diffTotalUnder > maxDiff) maxDiff = diffTotalUnder;

            if (maxDiff == diffTotalOver)
                data.CalculatedProbabilities.TotalOverProbability *= H2HCoefLastGames;
            if (maxDiff == diffTotalUnder)
                data.CalculatedProbabilities.TotalUnderProbability *= H2HCoefLastGames;
        }

        #endregion H2H

        #region Team Forms

        public static void CheckTeamForms(PredictionData data, Game game)
        {
            var leagueRepo = new LeagueDataRepository();
            var analysisRepo = new AnalysisDataRepository();

            var homeTeamId = game.HomeTeamId;
            var awayTeamId = game.AwayTeamId;

            //home team
            var homeTeamForm5 = analysisRepo.GetFootballTeamFormByIdAndType(game.SeasonRound.LeagueSeasonId, homeTeamId, TimePeriod.Last5Matches);
            var wins = homeTeamForm5.WinsPercentage;
            var losses = homeTeamForm5.LossesPercentage;
            var draws = homeTeamForm5.DrawsPercentage;
            var medianProbability = Math.Max(Math.Min(wins, losses), Math.Min(Math.Max(wins, losses), draws));

            if (wins > medianProbability)
                data.CalculatedProbabilities.HomeWinProbability *= TeamFormCoef;
            else if (wins < medianProbability)
                data.CalculatedProbabilities.HomeWinProbability /= TeamFormCoef;

            if (losses > medianProbability)
                data.CalculatedProbabilities.AwayWinProbability *= TeamFormCoef;
            else if (losses < medianProbability)
                data.CalculatedProbabilities.AwayWinProbability /= TeamFormCoef;

            if (draws > medianProbability)
                data.CalculatedProbabilities.DrawProbability *= TeamFormCoef;
            else if (draws < medianProbability)
                data.CalculatedProbabilities.DrawProbability /= TeamFormCoef;

            var bttsYes = (double)homeTeamForm5.BTTS_Yes / homeTeamForm5.GamePlayed;
            var bttsNo = (double)homeTeamForm5.BTTS_No / homeTeamForm5.GamePlayed;

            if(bttsYes > bttsNo)
            {
                data.CalculatedProbabilities.BttsYesProbability *= TeamFormCoef;
                data.CalculatedProbabilities.BttsNoProbability /= TeamFormCoef;
            }
            else
            {
                data.CalculatedProbabilities.BttsNoProbability *= TeamFormCoef;
                data.CalculatedProbabilities.BttsYesProbability /= TeamFormCoef;
            }

            var totalOver = (double)homeTeamForm5.GamesOver2_5 / homeTeamForm5.GamePlayed;
            var totalUnder = (double)homeTeamForm5.GamesUnder2_5 / homeTeamForm5.GamePlayed;

            if (totalOver > totalUnder)
            {
                data.CalculatedProbabilities.TotalOverProbability *= TeamFormCoef;
                data.CalculatedProbabilities.TotalUnderProbability /= TeamFormCoef;
            }
            else
            {
                data.CalculatedProbabilities.TotalUnderProbability *= TeamFormCoef;
                data.CalculatedProbabilities.TotalOverProbability /= TeamFormCoef;
            }

            //away team
            var awayTeamForm5 = analysisRepo.GetFootballTeamFormByIdAndType(game.SeasonRound.LeagueSeasonId, awayTeamId, TimePeriod.Last5Matches);
            wins = awayTeamForm5.WinsPercentage;
            losses = awayTeamForm5.LossesPercentage;
            draws = awayTeamForm5.DrawsPercentage;
            medianProbability = Math.Max(Math.Min(wins, losses), Math.Min(Math.Max(wins, losses), draws));

            if (wins > medianProbability)
                data.CalculatedProbabilities.AwayWinProbability *= TeamFormCoef;
            else if (wins < medianProbability)
                data.CalculatedProbabilities.AwayWinProbability /= TeamFormCoef;

            if (losses > medianProbability)
                data.CalculatedProbabilities.HomeWinProbability *= TeamFormCoef;
            else if (losses < medianProbability)
                data.CalculatedProbabilities.HomeWinProbability /= TeamFormCoef;

            if (draws > medianProbability)
                data.CalculatedProbabilities.DrawProbability *= TeamFormCoef;
            else if (draws < medianProbability)
                data.CalculatedProbabilities.DrawProbability /= TeamFormCoef;

            bttsYes = (double)awayTeamForm5.BTTS_Yes / awayTeamForm5.GamePlayed;
            bttsNo = (double)awayTeamForm5.BTTS_No / awayTeamForm5.GamePlayed;

            if (bttsYes > bttsNo)
            {
                data.CalculatedProbabilities.BttsYesProbability *= TeamFormCoef;
                data.CalculatedProbabilities.BttsNoProbability /= TeamFormCoef;
            }
            else
            {
                data.CalculatedProbabilities.BttsNoProbability *= TeamFormCoef;
                data.CalculatedProbabilities.BttsYesProbability /= TeamFormCoef;
            }

            totalOver = (double)awayTeamForm5.GamesOver2_5 / awayTeamForm5.GamePlayed;
            totalUnder = (double)awayTeamForm5.GamesUnder2_5 / awayTeamForm5.GamePlayed;

            if (totalOver > totalUnder)
            {
                data.CalculatedProbabilities.TotalOverProbability *= TeamFormCoef;
                data.CalculatedProbabilities.TotalUnderProbability /= TeamFormCoef;
            }
            else
            {
                data.CalculatedProbabilities.TotalUnderProbability *= TeamFormCoef;
                data.CalculatedProbabilities.TotalOverProbability /= TeamFormCoef;
            }
        }
        #endregion Team Forms

        #region SeasonStreaks

        public static void CheckSeasonStreaks(PredictionData data, Game game)
        {
            var leagueRepo = new LeagueDataRepository();
            var analysisRepo = new AnalysisDataRepository();
            
            var seasonStats = analysisRepo.GetSeasonStats(game.SeasonRound.LeagueSeasonId);
            var allSeasonGames = leagueRepo.GetAllSeasonGames(game.SeasonRound.LeagueSeasonId).Where(g => g.Result != 0).OrderByDescending(g => g.Date).ToList();
            
            //var homeTeamId = game.HomeTeamId;
            //var awayTeamId = game.AwayTeamId;
            int lastHomeTeamWinGamesAgo = -1, lastAwayTeamWinGamesAgo = -1, lastDrawGamesAgo = -1;
            int lastBttsYes = -1, lastBttsNo = -1;
            int lastTotalOver = -1, lastTotalUnder = -1;
            for (int i = 0; i < allSeasonGames.Count; i++)
            {
                if (lastHomeTeamWinGamesAgo > -1 && lastAwayTeamWinGamesAgo > -1 && lastDrawGamesAgo > -1
                    && lastTotalOver > -1 && lastTotalUnder > -1 && lastBttsYes > -1 && lastBttsNo > -1)
                    break;
                var tempGame = allSeasonGames[i];
                switch (tempGame.Result)
                {
                    case GameResult.HomeWin:
                        if (lastHomeTeamWinGamesAgo == -1)
                            lastHomeTeamWinGamesAgo = i;
                        break;
                    case GameResult.Draw:
                        if (lastDrawGamesAgo == -1)
                            lastDrawGamesAgo = i;
                        break;
                    case GameResult.AwayWin:
                        if (lastAwayTeamWinGamesAgo == -1)
                            lastAwayTeamWinGamesAgo = i;
                        break;
                }

                var totalOver = (tempGame.AwayTeamGoals + tempGame.HomeTeamGoals) > 2;
                if (totalOver && (lastTotalOver == -1)) lastTotalOver = i;
                if (!totalOver && (lastTotalUnder == -1)) lastTotalUnder = i;

                var bttsYes = (tempGame.AwayTeamGoals > 0) && (tempGame.HomeTeamGoals > 0);
                if (bttsYes && (lastBttsYes == -1)) lastBttsYes = i;
                if (!bttsYes && (lastBttsNo == -1)) lastBttsNo = i;
            }

            var averageHomeTeamWinFreq = 1 / seasonStats.HomeWinsPercentage;
            var averageAwayTeamWinFreq = 1 / seasonStats.AwayWinsPercentage;
            var averageDrawsFreq = 1 / seasonStats.DrawsPercentage;

            var averageBttsYesFreq = (double)seasonStats.GamePlayed / seasonStats.BTTS_Yes;
            var averageBttsNoFreq = (double)seasonStats.GamePlayed / seasonStats.BTTS_No;

            var averageTotalOverFreq = (double)seasonStats.GamePlayed / seasonStats.GamesOver2_5;
            var averageTotalUnderFreq = (double)seasonStats.GamePlayed / seasonStats.GamesUnder2_5;

            var minFreq = averageHomeTeamWinFreq;
            if (averageAwayTeamWinFreq < minFreq) minFreq = averageAwayTeamWinFreq;
            if (averageDrawsFreq < minFreq) minFreq = averageDrawsFreq;

            if (minFreq == averageHomeTeamWinFreq)
                data.CalculatedProbabilities.HomeWinProbability *= SeasonStatsCoef;
            if (minFreq == averageDrawsFreq)
                data.CalculatedProbabilities.DrawProbability *= SeasonStatsCoef;
            if (minFreq == averageAwayTeamWinFreq)
                data.CalculatedProbabilities.AwayWinProbability *= SeasonStatsCoef;

            minFreq = averageBttsYesFreq;
            if (averageBttsNoFreq < minFreq) minFreq = averageBttsNoFreq;

            if (minFreq == averageBttsNoFreq)
                data.CalculatedProbabilities.BttsNoProbability *= SeasonStatsCoef;
            if (minFreq == averageBttsYesFreq)
                data.CalculatedProbabilities.BttsYesProbability *= SeasonStatsCoef;

            minFreq = averageTotalOverFreq;
            if (averageTotalUnderFreq < minFreq) minFreq = averageTotalUnderFreq;

            if (minFreq == averageTotalOverFreq)
                data.CalculatedProbabilities.TotalOverProbability *= SeasonStatsCoef;
            if (minFreq == averageTotalUnderFreq)
                data.CalculatedProbabilities.TotalUnderProbability *= SeasonStatsCoef;

            //differences between last time some result happend and average result frequency 
            var diffHomeTeamWins = lastHomeTeamWinGamesAgo - averageHomeTeamWinFreq;
            var diffAwayTeamWins = lastAwayTeamWinGamesAgo - averageAwayTeamWinFreq;
            var diffDraws = lastDrawGamesAgo - averageDrawsFreq;

            var diffBttsYes = lastBttsYes - averageBttsYesFreq;
            var diffBttsNo = lastBttsNo - averageBttsNoFreq;

            var diffTotalOver = lastTotalOver - averageTotalOverFreq;
            var diffTotalUnder = lastTotalUnder - averageTotalUnderFreq;

            data.CalculatedProbabilities.HomeWinProbability *= GetCoefByDifference(diffHomeTeamWins);
            data.CalculatedProbabilities.AwayWinProbability *= GetCoefByDifference(diffAwayTeamWins);
            data.CalculatedProbabilities.DrawProbability *= GetCoefByDifference(diffDraws);

            data.CalculatedProbabilities.BttsYesProbability *= GetCoefByDifference(diffBttsYes);
            data.CalculatedProbabilities.BttsNoProbability *= GetCoefByDifference(diffBttsNo);

            data.CalculatedProbabilities.TotalOverProbability *= GetCoefByDifference(diffTotalOver);
            data.CalculatedProbabilities.TotalUnderProbability *= GetCoefByDifference(diffTotalUnder);
        }

        private static double GetCoefByDifference(double diff)
        {
            if (diff > 0)
            {
                if (diff < 1) diff += 1;
                var coef = Math.Pow(StreakBreakerCoef, diff);
                return coef;
            }
            else if (diff < 0)
            {
                diff = -diff;
                if (diff < 1) diff += 1;
                var coef = Math.Pow(StreakBreakerCoef, diff);
                return 1 / coef;
            }
            else return 1;
        }
        #endregion SeasonStreaks
    }
}

using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.LeagueData;
using FIT.Diploma.Server.Database.SystemData;
using System;
using System.Collections.Generic;
using System.Linq;
using FIT.Diploma.Server.Database.AnalyzerResults;

namespace FIT.Diploma.Server.DataAnalysis
{
    public class GamePredictionAnalyzer : IDataAnalyzer
    {
        private LeagueDataRepository leagueRepo = new LeagueDataRepository();
        private AnalysisDataRepository analysisRepo = new AnalysisDataRepository();
        private BookmakerOddsDataRepository bookRepo = new BookmakerOddsDataRepository();

        

        public string GetTargetDataTable()
        {
            return "LeagueSeasonTeams";
        }

        public void RunAnalyzing()
        {
            //Evaluate previous prediction
            EvaluatePredictions();

            //get all predictions
            var predictions = analysisRepo.GetAllPredictions();

            //get next games
            var nextGames = leagueRepo.GetAllUpcomingGames();

            //for each next game make prediction
            foreach(var game in nextGames)
            {
                try
                {
                    //if (predictions.Any(p => p.GameId == game.GameId)) continue;
                    var prediction = MakePrediction(game);
                    analysisRepo.AddOrUpdatePrediction(prediction);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"[GamePredictionAnalyzer] Thrown Exception: {ex.Message}");
                }
            }
        }

        public Prediction MakePrediction(Game game)
        {
            Prediction prediction = new Prediction { GameId = game.GameId };
            var gameOdds = bookRepo.GetLatestBookmakerOddsForGame(game.GameId);

            if (gameOdds == null)
                throw new Exception("BookmakerOdds not available.");

            PredictionData data = new PredictionData();
            data.OriginBookmakerOdds = new PredictionBookmakerOdds()
            {
                HomeWinCoef = gameOdds.HomeWinCoef,
                DrawCoef = gameOdds.DrawCoef,
                AwayWinCoef = gameOdds.AwayWinCoef,
                BttsNoCoef = gameOdds.BothTeamsToScore_No,
                BttsYesCoef = gameOdds.BothTeamsToScore_Yes,
                TotalOverCoef = gameOdds.Total2_5Over,
                TotalUnderCoef = gameOdds.Total2_5Under
            };

            data.OriginProbabilities = PredictionData.CalculateProbabilitiesFromBookmakerOdds(data.OriginBookmakerOdds);
            data.CalculatedProbabilities = new PredictionProbabilities(data.OriginProbabilities);
            
            //apply data to probabilities
            PredictionHelper.CheckRoundStats(data, game);
            PredictionHelper.CheckH2H(data, game);
            PredictionHelper.CheckTeamForms(data, game);
            PredictionHelper.CheckSeasonStreaks(data, game);

            data.CalculatedBookmakerOdds = PredictionData.CalculateBookmakerOddsFromProbabilities(data.CalculatedProbabilities,
                            data.OriginProbabilities.GetMainMargin(), data.OriginProbabilities.GetBttsMargin(), data.OriginProbabilities.GetTotalMargin());

            //make decision
            Random rand = new Random();
            double maxDiffProbability;
            double resultProbability;
            double optionCoef;
            var option = PredictionHelper.MakeDecision(data, out maxDiffProbability, out resultProbability, out optionCoef);

            double maxProbability;
            double originOptionCoef;
            var originalOption = PredictionHelper.GetOptionWithMaxPropability(data, out maxProbability, out originOptionCoef);

            if(maxProbability > resultProbability)
            {
                if ((maxProbability - resultProbability > 20) && (rand.NextDouble() >= 0.5 || optionCoef == 0))
                {
                    option = originalOption;
                    optionCoef = originOptionCoef;
                }
                else
                    Console.WriteLine($"Choosen option with less probability then best. Best option: {originalOption} [{originOptionCoef}]. Choosen option: {option} [{optionCoef}]");
            }
            

            prediction.PredictionOption = option;
            prediction.PredictionOptionCoef = prediction.PredictionOptionCoefMax = prediction.PredictionOptionCoefMin = optionCoef;
            return prediction;
        }

        private void EvaluatePredictions()
        {
            var predictions = analysisRepo.GetAllPredictions().Where(p => !p.IsFinished).ToList();

            foreach(var prediction in predictions)
            {
                var game = prediction.Game;
                if (game.Result == 0) continue;

                switch (prediction.PredictionOption)
                {
                    case PredictionOption.HomeWin:
                        prediction.IsSucceed = game.Result == GameResult.HomeWin;
                        break;
                    case PredictionOption.Draw:
                        prediction.IsSucceed = game.Result == GameResult.Draw;
                        break;
                    case PredictionOption.AwayWin:
                        prediction.IsSucceed = game.Result == GameResult.AwayWin;
                        break;
                    case PredictionOption.BttsYes:
                        prediction.IsSucceed = game.HomeTeamGoals > 0 && game.AwayTeamGoals > 0;
                        break;
                    case PredictionOption.BttsNo:
                        prediction.IsSucceed = game.HomeTeamGoals == 0 || game.AwayTeamGoals == 0;
                        break;
                    case PredictionOption.TotalOver2_5:
                        prediction.IsSucceed = (game.HomeTeamGoals + game.AwayTeamGoals) > 2;
                        break;
                    case PredictionOption.TotalUnder2_5:
                        prediction.IsSucceed = (game.HomeTeamGoals + game.AwayTeamGoals) <= 2;
                        break;

                    default:
                        throw new Exception("Unknown PredictionOption.");
                }

                prediction.IsFinished = true;
                //to do: update coefs stats
                analysisRepo.AddOrUpdatePrediction(prediction);
            }            
        }
    }    
}

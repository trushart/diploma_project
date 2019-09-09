using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FIT.Diploma.Shared.DataAccess.Dto;
using FIT.Diploma.REST.Client;
using FIT.Diploma.WebApplication.Models;
using System.IO;

namespace FIT.Diploma.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private IRestClient client = new RestClient();

        public ActionResult LeagueInformation(int selectedLeagueId = -1, int selectedSeasonId = -1)
        {
            LeagueInformationModel model = new LeagueInformationModel();
            try
            {
                model.Leagues = client.GetAllLeagues();
                model.SelectedLeague = selectedLeagueId == -1
                        ? model.Leagues.FirstOrDefault()
                        : model.Leagues.Where(l => l.LeagueId == selectedLeagueId).FirstOrDefault();
                selectedLeagueId = model.SelectedLeague?.LeagueId ?? -1;

                if (selectedLeagueId == -1)
                    return View(model);

                model.Seasons = client.GetAllLeagueSeasons(selectedLeagueId).OrderByDescending(s => s.StartYear).ToList();
                model.SelectedSeason = selectedSeasonId == -1
                        ? model.Seasons.FirstOrDefault()
                        : model.Seasons.Where(s => s.LeagueSeasonId == selectedSeasonId).FirstOrDefault();
                selectedSeasonId = model.SelectedSeason?.LeagueSeasonId ?? -1;

                if (selectedSeasonId == -1)
                    return View(model);

                var allSeasonRounds = client.GetSeasonRounds(selectedSeasonId).OrderByDescending(sr => sr.RoundNumber).ToList();
                model.CurrentRoundGames = client.GetCurrentRoundGames(selectedSeasonId);
                model.CurrentRound = allSeasonRounds.Where(r => r.RoundId == (model.CurrentRoundGames.FirstOrDefault()?.RoundId ?? -1)).FirstOrDefault();

                if(allSeasonRounds.Count > 1)
                {
                    var previousRound = allSeasonRounds.Where(r => r.IsFinished).FirstOrDefault();
                    if(previousRound != null)
                    {
                        model.PreviousRoundGames = client.GetRoundGames(selectedSeasonId, previousRound.RoundId);
                        model.PreviousRound = previousRound;
                    }                    
                }

                model.StandingTable = client.GetStandingTable(selectedSeasonId);
            }
            catch(Exception ex)
            {
                //log exception
                //return ErrorPage
                model.Error = ex.Message;
                return View(model);
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult GetGameStats(int seasonId, int gameId, int team1Id, int team2Id)
        {
            ReturnArgs r = new ReturnArgs();

            var model = new GameStatsModel()
            {
                GameInfo = client.GetGameStats(seasonId, gameId),
                HeadToHead = client.GetH2HStats(seasonId, team1Id, team2Id)
            };
            r.ViewString = RenderRazorViewToString("_GameStats", model);

            return Json(r);
        }

        public ActionResult SearchingTool()
        {
            var model = new SearchingToolModel();
            model.RangeTypes = new List<RangeType>()
            {
                new RangeType
                {
                    RangeTypeId = RangeTypeId.DateTime,
                    RangeTypeName = "Dates"
                }
            };
            model.ResultFormats = new List<ResultFormat>()
            {
                new ResultFormat
                {
                    ResultFormatId = ResultFormatId.AllGames,
                    ResultFormatName = "All Games"
                },
                new ResultFormat
                {
                    ResultFormatId = ResultFormatId.NumberOfAllGames,
                    ResultFormatName = "All Games Number"
                },
                new ResultFormat
                {
                    ResultFormatId = ResultFormatId.MinStreak,
                    ResultFormatName = "Minimal Streak"
                },
                new ResultFormat
                {
                    ResultFormatId = ResultFormatId.MaxStreak,
                    ResultFormatName = "Maximal Streak"
                },
                new ResultFormat
                {
                    ResultFormatId = ResultFormatId.NumberOfStreaks,
                    ResultFormatName = "Streaks Number"
                }
            };
            model.TotalTypes = new List<Condition>()
            {
                new Condition
                {
                    TotalType = TotalType.Over
                },
                new Condition
                {
                    TotalType = TotalType.Equel
                }
                ,
                new Condition
                {
                    TotalType = TotalType.Under
                }
            };
            model.ConditionTypes = new List<Condition>()
            {
                new Condition
                {
                    ConditionTypeId = ConditionTypeId.Btts
                },
                new Condition
                {
                    ConditionTypeId = ConditionTypeId.GamePlace
                },
                new Condition
                {
                    ConditionTypeId = ConditionTypeId.GameResult
                },
                new Condition
                {
                    ConditionTypeId = ConditionTypeId.GameTotal
                },
                new Condition
                {
                    ConditionTypeId = ConditionTypeId.Team
                },
                new Condition
                {
                    ConditionTypeId = ConditionTypeId.TeamTotal
                }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GetSearchingToolResult(SearchingToolInput input)
        {
            ReturnArgs r = new ReturnArgs();

            var condition = new SearchToolConditionsDto
            {
                TimeRange = new SearchTimeRangeDto
                {
                    FromDate = input.DateFrom,
                    ToDate = input.DateTo
                }
            };
            foreach(var cond in input.Conditions)
            {
                switch (cond.ConditionTypeId)
                {                    
                    case ConditionTypeId.GameTotal:
                        condition.GameTotal = new ST_Total
                        {
                            GoalsNumber = Int32.Parse(cond.SelectedCondition),
                            TotalType = (ST_TotalType)((int)cond.TotalType)
                        };
                        break;
                    case ConditionTypeId.TeamTotal:
                        condition.TeamTotal = new ST_TeamTotal
                        {
                            GoalsNumber = Int32.Parse(cond.SelectedCondition),
                            TotalType = (ST_TotalType)((int)cond.TotalType),
                            Team = ST_Team.Team1
                        };
                        break;
                    case ConditionTypeId.Btts:
                        condition.BothTeamsScore = bool.Parse(cond.SelectedCondition);
                        break;
                    case ConditionTypeId.Team:
                        condition.TeamId = 16;
                        break;
                    case ConditionTypeId.GameResult:
                        condition.Result = (ST_GameResultDto)(int.Parse(cond.SelectedCondition));
                        break;
                    case ConditionTypeId.GamePlace:
                        condition.GamePlace = (ST_GamePlaceDto)(int.Parse(cond.SelectedCondition));
                        break;                   
                    default:
                        break;
                }
            }

            var model = new SearchingToolResults()
            {
                StreaksNumber = -1,
                GamesNumber = -1,
                Games = new List<GameDto>()
            };

            switch (input.ResultFormatId)
            {
                case ResultFormatId.AllGames:
                    model.Games = client.GetAllGames(condition);
                    break;
                case ResultFormatId.NumberOfAllGames:
                    model.GamesNumber = client.GetNumberAllGames(condition);
                    break;
                case ResultFormatId.MinStreak:
                    model.Games = client.GetMinStreak(condition);
                    break;
                case ResultFormatId.MaxStreak:
                    model.Games = client.GetMaxStreak(condition);
                    break;
                case ResultFormatId.NumberOfStreaks:
                    model.StreaksNumber = 777;
                    break;
                default:
                    throw new Exception("Not implemented!!!");
            }
            
            r.ViewString = RenderRazorViewToString("_SearchingToolResult", model);

            return Json(r);
        }

        [HttpPost]
        public ActionResult GetSearchingToolConditions(List<Condition> conditions)
        {
            ReturnArgs r = new ReturnArgs();

            var model = new SearchingToolConditionsModel()
            {
                Conditions = conditions
            };
            r.ViewString = RenderRazorViewToString("_SearchingToolConditions", model);

            return Json(r);
        }

        public class ReturnArgs
        {
            public ReturnArgs()
            {
            }
            public string ViewString { get; set; }
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult Predictions(int selectedLeagueId = -1, int selectedSeasonId = -1, PredictionsType type = PredictionsType.All)
        {
            PredictionsModel model = new PredictionsModel();
            try
            {
                model.Leagues = client.GetAllLeagues();
                model.SelectedLeague = selectedLeagueId == -1
                        ? model.Leagues.FirstOrDefault()
                        : model.Leagues.Where(l => l.LeagueId == selectedLeagueId).FirstOrDefault();
                selectedLeagueId = model.SelectedLeague?.LeagueId ?? -1;

                if (selectedLeagueId == -1)
                    return View(model);

                model.Seasons = client.GetAllLeagueSeasons(selectedLeagueId).OrderByDescending(s => s.StartYear).ToList();
                model.SelectedSeason = selectedSeasonId == -1
                        ? model.Seasons.FirstOrDefault()
                        : model.Seasons.Where(s => s.LeagueSeasonId == selectedSeasonId).FirstOrDefault();
                selectedSeasonId = model.SelectedSeason?.LeagueSeasonId ?? -1;

                if (selectedSeasonId == -1)
                    return View(model);

                PredictionsResponseDto predictionResponse = null;
                switch (type)
                {
                    case PredictionsType.All:
                        predictionResponse = client.GetAllSeasonPredictions(selectedSeasonId);                        
                        break;
                    case PredictionsType.Current:
                        predictionResponse = client.GetCurrentSeasonPredictions(selectedSeasonId);
                        break;
                    case PredictionsType.Finished:
                        predictionResponse = client.GetFinishedSeasonPredictions(selectedSeasonId);
                        break;
                    default:
                        throw new Exception("Unknown PredictionsType");
                }

                model.SelectedPredictionsType = type;
                model.Predictions = predictionResponse.Predictions;
            }
            catch (Exception ex)
            {
                //log exception
                //return ErrorPage
                model.Error = ex.Message;
                return View(model);
            }
            return View(model);
        }
    }
}
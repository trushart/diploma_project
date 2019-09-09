using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;
using FIT.Diploma.Server.DataAccess.Repositories;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbGameResult = FIT.Diploma.Server.Database.LeagueData.GameResult;

namespace FIT.Diploma.Server.DataAnalysis.SearchingTool
{
    public class SearchEngine : ISearchEngine
    {
        private ILeagueDataRepository leagueRepo;
        public SearchEngine()
        {
            leagueRepo = new LeagueDataRepository();
        }

        //for testing
        public SearchEngine(ILeagueDataRepository repo)
        {
            leagueRepo = repo;
        }

        public List<Game> GetGames(SearchConditions conditions)
        {            
            var result = leagueRepo.GetAllGamesInTimeRange(conditions.TimeRange.FromDate, conditions.TimeRange.ToDate);

            if (conditions.TeamId.HasValue)
            {
                var teamId = conditions.TeamId.Value;

                //gameplaces
                if (conditions.GamePlace == GamePlace.NotDefined)
                    result = result.Where(g => g.HomeTeamId == teamId || g.AwayTeamId == teamId).ToList();
                else if (conditions.GamePlace == GamePlace.Home)
                    result = result.Where(g => g.HomeTeamId == teamId).ToList();
                else if (conditions.GamePlace == GamePlace.Away)
                    result = result.Where(g => g.AwayTeamId == teamId).ToList();


                //Result of game
                if (conditions.Result != GameResult.NotDefined)
                {
                    if (conditions.Result == GameResult.Draw)
                        result = result.Where(g => g.Result == DbGameResult.Draw
                                    && (g.HomeTeamId == teamId || g.AwayTeamId == teamId)
                                    ).ToList();
                    else if (conditions.Result == GameResult.Team1Win) //wins of teamId
                        result = result.Where(g => 
                                    (g.Result == DbGameResult.HomeWin && g.HomeTeamId == teamId)
                                    ||
                                    (g.Result == DbGameResult.AwayWin && g.AwayTeamId == teamId)
                                    ).ToList();
                    else if (conditions.Result == GameResult.Team2Win) //loses of teamId
                        result = result.Where(g =>
                                    (g.Result == DbGameResult.HomeWin && g.AwayTeamId == teamId)
                                    ||
                                    (g.Result == DbGameResult.AwayWin && g.HomeTeamId == teamId)
                                    ).ToList();
                }
                
                //team total
                if (conditions.TeamTotal != null && conditions.TeamTotal.TotalType != TotalType.NotDefined)
                {
                    var teamTotal = conditions.TeamTotal.GoalsNumber;
                    if(conditions.TeamTotal.Team == Team.Team1) //total of goals scored by teamId
                    {
                        if (conditions.TeamTotal.TotalType == TotalType.Over)
                            result = result.Where(g =>
                                                (g.AwayTeamGoals > teamTotal && g.AwayTeamId == teamId)
                                                ||
                                                (g.HomeTeamGoals > teamTotal && g.HomeTeamId == teamId)
                                                ).ToList();
                        else if (conditions.TeamTotal.TotalType == TotalType.Under)
                            result = result.Where(g =>
                                                (g.AwayTeamGoals < teamTotal && g.AwayTeamId == teamId)
                                                ||
                                                (g.HomeTeamGoals < teamTotal && g.HomeTeamId == teamId)
                                                ).ToList();
                        else //equal
                            result = result.Where(g =>
                                                (g.AwayTeamGoals == teamTotal && g.AwayTeamId == teamId)
                                                ||
                                                (g.HomeTeamGoals == teamTotal && g.HomeTeamId == teamId)
                                                ).ToList();
                    }
                    else //total of goals conceded by teamId
                    {
                        if (conditions.TeamTotal.TotalType == TotalType.Over)
                            result = result.Where(g =>
                                                (g.AwayTeamGoals > teamTotal && g.HomeTeamId == teamId)
                                                ||
                                                (g.HomeTeamGoals > teamTotal && g.AwayTeamId == teamId)
                                                ).ToList();
                        else if (conditions.TeamTotal.TotalType == TotalType.Under)
                            result = result.Where(g =>
                                                (g.AwayTeamGoals < teamTotal && g.HomeTeamId == teamId)
                                                ||
                                                (g.HomeTeamGoals < teamTotal && g.AwayTeamId == teamId)
                                                ).ToList();
                        else //equal
                            result = result.Where(g =>
                                                (g.AwayTeamGoals == teamTotal && g.HomeTeamId == teamId)
                                                ||
                                                (g.HomeTeamGoals == teamTotal && g.AwayTeamId == teamId)
                                                ).ToList();
                    }                    
                }                
            }
            else
            {
                //Result of game
                if (conditions.Result != GameResult.NotDefined)
                {
                    if (conditions.Result == GameResult.Draw)
                        result = result.Where(g => g.Result == DbGameResult.Draw).ToList();
                    else if (conditions.Result == GameResult.Team1Win)
                        result = result.Where(g => g.Result == DbGameResult.HomeWin).ToList();
                    else if (conditions.Result == GameResult.Team2Win)
                        result = result.Where(g => g.Result == DbGameResult.AwayWin).ToList();
                }

                if (conditions.TeamTotal != null && conditions.TeamTotal.TotalType != TotalType.NotDefined)
                {
                    var teamTotal = conditions.TeamTotal.GoalsNumber;
                    if (conditions.TeamTotal.Team == Team.Team1) //total of goals scored by homeTeam
                    {
                        if (conditions.TeamTotal.TotalType == TotalType.Over)
                            result = result.Where(g => g.HomeTeamGoals > teamTotal).ToList();
                        else if (conditions.TeamTotal.TotalType == TotalType.Under)
                            result = result.Where(g => g.HomeTeamGoals < teamTotal).ToList();
                        else //equal
                            result = result.Where(g => g.HomeTeamGoals == teamTotal).ToList();
                    }
                    else //total of goals scored by awayTeam
                    {
                        if (conditions.TeamTotal.TotalType == TotalType.Over)
                            result = result.Where(g => g.AwayTeamGoals > teamTotal).ToList();
                        else if (conditions.TeamTotal.TotalType == TotalType.Under)
                            result = result.Where(g => g.AwayTeamGoals < teamTotal).ToList();
                        else //equal
                            result = result.Where(g => g.AwayTeamGoals == teamTotal).ToList();
                    }
                }
            }

            //BothTeamsScore
            if (conditions.BothTeamsScore.HasValue)
            {
                if (conditions.BothTeamsScore.Value)
                    result = result.Where(g => g.HomeTeamGoals > 0 && g.AwayTeamGoals > 0).ToList();
                else
                    result = result.Where(g => g.HomeTeamGoals == 0 || g.AwayTeamGoals == 0).ToList();
            }

            //Game total
            if(conditions.GameTotal != null && conditions.GameTotal.TotalType != TotalType.NotDefined)
            {
                var total = conditions.GameTotal.GoalsNumber;
                if (conditions.GameTotal.TotalType == TotalType.Over)
                    result = result.Where(g => (g.AwayTeamGoals + g.HomeTeamGoals) > total).ToList();
                else if (conditions.GameTotal.TotalType == TotalType.Under)
                    result = result.Where(g => (g.AwayTeamGoals + g.HomeTeamGoals) < total).ToList();
                else //equal
                    result = result.Where(g => (g.AwayTeamGoals + g.HomeTeamGoals) == total).ToList();
            }

            return result;
        }

        public int GetGamesNumber(SearchConditions conditions)
        {
            return GetGames(conditions).Count;
        }

        public List<Game> GetMaximalStreak(SearchConditions conditions)
        {
            var streakInfos = ComputeStreaks(conditions);
            var lastItemId_maxStreak = streakInfos.GetLastItemOfMaximalStreak();
            if (lastItemId_maxStreak == -1)
                return null;

            List<Game> result = new List<Game>();
            for(int i = lastItemId_maxStreak - streakInfos.StreakLasting[lastItemId_maxStreak]; i <= lastItemId_maxStreak; i++)
            {
                result.Add(streakInfos.OrderedGames[i]);
            }

            return result;
        }

        public List<Game> GetMinimalStreak(SearchConditions conditions)
        {
            var streakInfos = ComputeStreaks(conditions);
            var lastItemId_minStreak = streakInfos.GetLastItemOfMinimalStreak();
            if (lastItemId_minStreak == -1)
                return null;

            List<Game> result = new List<Game>();
            for (int i = lastItemId_minStreak - streakInfos.StreakLasting[lastItemId_minStreak]; i <= lastItemId_minStreak; i++)
            {
                result.Add(streakInfos.OrderedGames[i]);
            }

            return result;
        }

        public int GetNumberOfStreaks(SearchConditions conditions, StreakConditions streakConditions)
        {
            if (streakConditions.TotalType == TotalType.NotDefined)
                return -1;

            var streakInfos = ComputeStreaks(conditions);
            var result = 0;

            Func<int, bool> comparison;
            if (streakConditions.TotalType == TotalType.Under)
                comparison = a => a < streakConditions.NumberOfItems;
            else if (streakConditions.TotalType == TotalType.Over)
                comparison = a => a > streakConditions.NumberOfItems;
            else //equal
                comparison = a => a == streakConditions.NumberOfItems;

            for(int i = 0; i < streakInfos.StreakLasting.Length; i++)
                if (i == streakInfos.StreakLasting.Length - 1 
                    || (streakInfos.StreakLasting[i] != -1 && streakInfos.StreakLasting[i+1] == -1)) //if end of streak
                    if (comparison(streakInfos.StreakLasting[i] + 1)) result++;

            return result;
        }

        private StreakInfos ComputeStreaks(SearchConditions conditions)
        {
            var allGames = leagueRepo.GetAllGamesInTimeRange(conditions.TimeRange.FromDate, conditions.TimeRange.ToDate);

            if (conditions.TeamId.HasValue)
            {
                var teamId = conditions.TeamId.Value;

                //gameplaces
                if (conditions.GamePlace == GamePlace.NotDefined)
                    allGames = allGames.Where(g => g.HomeTeamId == teamId || g.AwayTeamId == teamId).ToList();
                else if (conditions.GamePlace == GamePlace.Home)
                    allGames = allGames.Where(g => g.HomeTeamId == teamId).ToList();
                else if (conditions.GamePlace == GamePlace.Away)
                    allGames = allGames.Where(g => g.AwayTeamId == teamId).ToList();
            }

            allGames = allGames.OrderBy(g => g.Date).ToList();

            var result = new StreakInfos
            {
                OrderedGames = allGames.ToArray(),
                StreakLasting = new int[allGames.Count]
            };

            for(int i = 0; i < result.StreakLasting.Length; i++)
            {
                if (CheckGameConditions(result.OrderedGames[i], conditions))
                {
                    if (i == 0) result.StreakLasting[i] = 0;
                    else result.StreakLasting[i] = result.StreakLasting[i - 1] + 1;
                }
                else result.StreakLasting[i] = -1;
            }

            return result;
        }

        private bool CheckGameConditions(Game game, SearchConditions conditions)
        {
            var result = true;

            if (conditions.TeamId.HasValue)
            {
                var teamId = conditions.TeamId.Value;

                //Result of game
                if (conditions.Result != GameResult.NotDefined)
                {
                    if (conditions.Result == GameResult.Draw)
                        result &= game.Result == DbGameResult.Draw;
                    else if (conditions.Result == GameResult.Team1Win) //wins of teamId
                        result &= ((game.Result == DbGameResult.HomeWin && game.HomeTeamId == teamId) 
                            || (game.Result == DbGameResult.AwayWin && game.AwayTeamId == teamId));
                    else if (conditions.Result == GameResult.Team2Win) //loses of teamId
                        result &= ((game.Result == DbGameResult.HomeWin && game.AwayTeamId == teamId)
                            || (game.Result == DbGameResult.AwayWin && game.HomeTeamId == teamId));
                }

                //team total
                if (conditions.TeamTotal != null && conditions.TeamTotal.TotalType != TotalType.NotDefined)
                {
                    var teamTotal = conditions.TeamTotal.GoalsNumber;
                    if (conditions.TeamTotal.Team == Team.Team1) //total of goals scored by teamId
                    {
                        if (conditions.TeamTotal.TotalType == TotalType.Over)
                            result &= ((game.AwayTeamGoals > teamTotal && game.AwayTeamId == teamId)
                           || (game.HomeTeamGoals > teamTotal && game.HomeTeamId == teamId));
                        else if (conditions.TeamTotal.TotalType == TotalType.Under)
                            result &= ((game.AwayTeamGoals < teamTotal && game.AwayTeamId == teamId)
                           || (game.HomeTeamGoals < teamTotal && game.HomeTeamId == teamId));
                        else //equal
                            result &= ((game.AwayTeamGoals == teamTotal && game.AwayTeamId == teamId)
                            || (game.HomeTeamGoals == teamTotal && game.HomeTeamId == teamId));
                       
                    }
                    else //total of goals conceded by teamId
                    {
                        if (conditions.TeamTotal.TotalType == TotalType.Over)
                            result &= ((game.AwayTeamGoals > teamTotal && game.HomeTeamId == teamId)
                           || (game.HomeTeamGoals > teamTotal && game.AwayTeamId == teamId));
                        else if (conditions.TeamTotal.TotalType == TotalType.Under)
                            result &= ((game.AwayTeamGoals < teamTotal && game.HomeTeamId == teamId)
                           || (game.HomeTeamGoals < teamTotal && game.AwayTeamId == teamId));
                        else //equal
                            result &= ((game.AwayTeamGoals == teamTotal && game.HomeTeamId == teamId)
                           || (game.HomeTeamGoals == teamTotal && game.AwayTeamId == teamId));
                    }
                }
            }
            else
            {
                //Result of game
                if (conditions.Result != GameResult.NotDefined)
                {
                    if (conditions.Result == GameResult.Draw)
                        result &= game.Result == DbGameResult.Draw;
                    else if (conditions.Result == GameResult.Team1Win)
                        result &= game.Result == DbGameResult.HomeWin;
                    else if (conditions.Result == GameResult.Team2Win)
                        result &= game.Result == DbGameResult.AwayWin;
                }

                if (conditions.TeamTotal != null && conditions.TeamTotal.TotalType != TotalType.NotDefined)
                {
                    var teamTotal = conditions.TeamTotal.GoalsNumber;
                    if (conditions.TeamTotal.Team == Team.Team1) //total of goals scored by homeTeam
                    {
                        if (conditions.TeamTotal.TotalType == TotalType.Over)
                            result &= game.HomeTeamGoals > teamTotal;
                        else if (conditions.TeamTotal.TotalType == TotalType.Under)
                            result &= game.HomeTeamGoals < teamTotal;
                        else //equal
                            result &= game.HomeTeamGoals == teamTotal;
                    }
                    else //total of goals scored by awayTeam
                    {
                        if (conditions.TeamTotal.TotalType == TotalType.Over)
                            result &= game.AwayTeamGoals > teamTotal;
                        else if (conditions.TeamTotal.TotalType == TotalType.Under)
                            result &= game.AwayTeamGoals < teamTotal;
                        else //equal
                            result &= game.AwayTeamGoals == teamTotal;
                    }
                }
            }

            //BothTeamsScore
            if (conditions.BothTeamsScore.HasValue)
            {
                if (conditions.BothTeamsScore.Value)
                    result &= (game.HomeTeamGoals > 0 && game.AwayTeamGoals > 0);
                else
                    result &= (game.HomeTeamGoals == 0 || game.AwayTeamGoals == 0);
            }

            //Game total
            if (conditions.GameTotal != null && conditions.GameTotal.TotalType != TotalType.NotDefined)
            {
                var total = conditions.GameTotal.GoalsNumber;
                if (conditions.GameTotal.TotalType == TotalType.Over)
                    result &= (game.HomeTeamGoals + game.AwayTeamGoals) > total;
                else if (conditions.GameTotal.TotalType == TotalType.Under)
                    result &= (game.HomeTeamGoals + game.AwayTeamGoals) < total;
                else //equal
                    result &= (game.HomeTeamGoals + game.AwayTeamGoals) == total;
            }

            return result;
        }

        private class StreakInfos
        {
            public Game[] OrderedGames { get; set; }
            public int[] StreakLasting { get; set; }

            public int GetLastItemOfMaximalStreak()
            {
                if (StreakLasting == null)
                    return -1;
                int tempMax = StreakLasting[0];
                int tempMaxId = 0;

                for(int i = 1; i < StreakLasting.Length; i++)
                    if(StreakLasting[i] > tempMax)
                    {
                        tempMax = StreakLasting[i];
                        tempMaxId = i;
                    }

                return tempMaxId;
            }

            public int GetLastItemOfMinimalStreak()
            {
                if (StreakLasting == null)
                    return -1;
                int tempMin = StreakLasting[StreakLasting.Length - 1] != -1
                    ? StreakLasting[StreakLasting.Length - 1]
                    : int.MaxValue;
                int tempMinId = -1;

                for (int i = 0; i < StreakLasting.Length - 1; i++)
                    if (StreakLasting[i] < tempMin && StreakLasting[i] != -1 && StreakLasting[i + 1] == -1)
                    {
                        tempMin = StreakLasting[i];
                        tempMinId = i;
                    }

                return tempMinId;
            }
        }
    }
}

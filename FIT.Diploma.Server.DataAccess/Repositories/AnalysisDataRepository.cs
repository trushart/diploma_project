using FIT.Diploma.Server.Database.AnalyzerResults;
using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataAccess.Repositories
{
    public class AnalysisDataRepository : BaseRepository
    {
        public List<LeagueSeasonTeams> GetRangeOfTeams(List<int> teamIds, LeagueSeason season)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var results = (from n in Db.LeagueSeasonTeams
                        where
                            teamIds.Contains(n.FootballTeamId) &&
                            n.LeagueSeasonId == season.LeagueSeasonId
                        select n).ToList();

            if (results.Count != teamIds.Count)
            {
                var repo = new LogRepository();
                foreach (int teamId in teamIds)
                {
                    if(results.FindIndex(i => i.FootballTeamId == teamId) == -1 )
                    {
                        //create new record in LeagueSeasonTeams table
                        var team = new LeagueSeasonTeams
                        {
                            FootballTeamId = teamId,
                            LeagueSeasonId = season.LeagueSeasonId,
                            LastUpdate = DateTime.Now                            
                        };

                        Db.LeagueSeasonTeams.Add(team);
                        Db.SaveChanges();

                        results.Add(team);

                        repo.WriteLog(Database.SystemData.Severity.Information, "Insert to LeagueSeasonTeams table new record", nameof(AnalysisDataRepository),
                                    "localhost", "[teamId = " + team.FootballTeamId + "][LeagueSeasonId = " + team.LeagueSeasonId + "]", "");
                    }
                }                
            }

            Db.Database.Connection.Close();
            return results;
        }

        public int GetSeasonTeamsCount(int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.LeagueSeasonTeams
                          where n.LeagueSeasonId == leagueSeasonId
                          select n).Count();

            Db.Database.Connection.Close();
            return result;
        }

        public List<LeagueSeasonTeams> GetSeasonTeams(int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.LeagueSeasonTeams
                           where n.LeagueSeasonId == leagueSeasonId
                           select n).ToList();

            Db.Database.Connection.Close();
            return result;
        }

        public List<Prediction> GetAllPredictions()
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.Predictions select n).ToList();

            Db.Database.Connection.Close();
            return result;
        }

        public List<Prediction> GetSeasonPredictions(int seasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.Predictions
                          where n.Game.SeasonRound.LeagueSeasonId == seasonId
                          orderby n.Game.Date descending
                          select n).ToList();

            Db.Database.Connection.Close();
            return result;
        }

        public void UpdateGameIdForPredictions(int oldGameId, int newGameId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var results = (from n in Db.Predictions
                          where n.GameId == oldGameId
                          select n).ToList();

            foreach (var game in results)
            {
                game.GameId = newGameId;

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update GameID in Predictions table", nameof(AnalysisDataRepository),
                            "localhost", "[OldGameId = " + oldGameId + "] [NewGameId" + newGameId + "]", "");
            }

            Db.Database.Connection.Close();
        }

        public void AddOrUpdatePrediction(Prediction prediction)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.Predictions
                          where n.GameId == prediction.GameId
                          select n).FirstOrDefault();

            if (result == null)
            {
                //add teamForm
                Db.Predictions.Add(prediction);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to Predictions table new record", nameof(AnalysisDataRepository),
                            "localhost", "[GameId = " + prediction.GameId + "]", "");
            }
            else
            {
                var oldPrediction = result.PredictionOption;
                var newPrediction = prediction.PredictionOption;
                //update teamForm
                result.Copy(prediction);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in Predictions table", nameof(AnalysisDataRepository),
                            "localhost", "[GameId = " + result.GameId + "][oldPrediction = " + oldPrediction + "][newPrediction = " + newPrediction + "]", "");
            }

            Db.Database.Connection.Close();
        }

        public AverageRoundStats GetAverageRoundStats(int seasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.AverageRoundStats
                          where n.LeagueSeasonId == seasonId
                          select n).FirstOrDefault();

            Db.Database.Connection.Close();
            return result;
        }

        public void AddOrUpdateAverageRoundStats(AverageRoundStats averageRoundStats)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.AverageRoundStats
                          where n.LeagueSeasonId == averageRoundStats.LeagueSeasonId
                          select n).FirstOrDefault();

            if (result == null)
            {
                //add teamForm
                Db.AverageRoundStats.Add(averageRoundStats);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to AverageRoundStats table new record", nameof(AnalysisDataRepository),
                            "localhost", "[LeagueSeasonId = " + averageRoundStats.LeagueSeasonId + "]", "");
            }
            else
            {
                //update teamForm
                result.Copy(averageRoundStats);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in AverageRoundStats table", nameof(AnalysisDataRepository),
                            "localhost", "[LeagueSeasonId = " + result.LeagueSeasonId + "]", "");
            }

            Db.Database.Connection.Close();
        }

        public void AddOrUpdateBookmakerOddsStats(BookmakerOddsStats oddsStats)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.BookmakerOddsStats
                          where n.GameId == oddsStats.GameId
                          select n).FirstOrDefault();

            if (result == null)
            {
                //add teamForm
                Db.BookmakerOddsStats.Add(oddsStats);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to BookmakerOddsStats table new record", nameof(AnalysisDataRepository),
                            "localhost", "[GameId = " + oddsStats.GameId + "]", "");
            }
            else
            {
                //update teamForm
                result.Copy(oddsStats);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in BookmakerOddsStats table", nameof(AnalysisDataRepository),
                            "localhost", "[GameId = " + result.GameId + "]", "");
            }

            Db.Database.Connection.Close();
        }

        public List<FootballTeamForm> GetSeasonTeamForms_AllGames(int seasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
            List<FootballTeamForm> results = new List<FootballTeamForm>();

            results = (from n in Db.FootballTeamForm where n.TimePeriod == TimePeriod.AllMatches 
                        && n.LeagueSeasonId == seasonId select n).ToList();

            Db.Database.Connection.Close();
            return results;
        }

        public List<FootballTeamForm> GetSeasonTeamForms_HomeGames(int seasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
            List<FootballTeamForm> results = new List<FootballTeamForm>();

            results = (from n in Db.FootballTeamForm where n.TimePeriod == TimePeriod.AllHomeMatches 
                        && n.LeagueSeasonId == seasonId select n).ToList();

            Db.Database.Connection.Close();
            return results;
        }

        public List<FootballTeamForm> GetSeasonTeamForms_AwayGames(int seasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
            List<FootballTeamForm> results = new List<FootballTeamForm>();

            results = (from n in Db.FootballTeamForm where n.TimePeriod == TimePeriod.AllAwayMatches 
                        && n.LeagueSeasonId == seasonId select n).ToList();

            Db.Database.Connection.Close();
            return results;
        }

        public FootballTeamForm GetFootballTeamFormByIdAndType(int seasonId, int teamId, TimePeriod timePeriod)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.FootballTeamForm
                       where n.TimePeriod == timePeriod && n.TeamId == teamId && n.LeagueSeasonId == seasonId
                       select n).FirstOrDefault();

            Db.Database.Connection.Close();
            return result;
        }

        public void AddOrUpdateTeamForm(FootballTeamForm teamForm)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.FootballTeamForm
                          where n.TeamId == teamForm.TeamId
                          && n.TimePeriod == teamForm.TimePeriod
                          && n.LeagueSeasonId == teamForm.LeagueSeasonId
                          select n).FirstOrDefault();

            if(result == null)
            {
                //add teamForm
                Db.FootballTeamForm.Add(teamForm);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to FootballTeamForm table new record", nameof(AnalysisDataRepository),
                            "localhost", "[teamId = " + teamForm.TeamId + "][LeagueSeasonId = "
                            + teamForm.LeagueSeasonId + "][TimePeriod = " + teamForm.TimePeriod + "]", "");
            }
            else
            {
                //update teamForm
                result.Copy(teamForm);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in FootballTeamForm table", nameof(AnalysisDataRepository),
                            "localhost", "[teamId = " + result.TeamId + "][LeagueSeasonId = " 
                            + result.LeagueSeasonId + "][TimePeriod = " + result.TimePeriod + "]", "");                
            }

            Db.Database.Connection.Close();
        }

        public void UpdateSeasonTeams(List<LeagueSeasonTeams> teams)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
            var repo = new LogRepository();

            foreach (var team in teams)
            {
                var teamDb = (from n in Db.LeagueSeasonTeams where n.FootballTeamId == team.FootballTeamId select n).FirstOrDefault();
                if (teamDb != null)
                {
                    teamDb.Copy(team);                    
                    repo.WriteLog(Database.SystemData.Severity.Information, "Insert to LeagueSeasonTeams table new record", nameof(AnalysisDataRepository),
                                    "localhost", "[teamId = " + team.FootballTeamId + "][LeagueSeasonId = " + team.LeagueSeasonId + "]", "");
                }
            }
            
            Db.SaveChanges();
            Db.Database.Connection.Close();
        }


        public SeasonStats GetSeasonStats(int leagueSeasonId)
        {
            OpenConnection();

            var seasonStats = Db.SeasonStats.Where(stats => stats.LeagueSeasonId == leagueSeasonId).FirstOrDefault();

            if (seasonStats == null)
            {
                seasonStats = new SeasonStats
                {
                    LeagueSeasonId = leagueSeasonId,
                    LastUpdate = DateTime.Now
                };

                Db.SeasonStats.Add(seasonStats);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to SeasonStats table new record", nameof(AnalysisDataRepository),
                            "localhost", "[LeagueSeasonId = " + seasonStats.LeagueSeasonId + "]", "");
            }

            Db.Database.Connection.Close();
            return seasonStats;
        }


        public HeadToHeadStats GetTeamsHeadToHead(int teamId1, int teamId2)
        {
            OpenConnection();
            var repo = new LogRepository();

            var headToheadStats = Db.HeadToHeadStats.Where(h2h => 
                    (h2h.Team1Id == teamId1 && h2h.Team2Id == teamId2)
                    ||
                    (h2h.Team1Id == teamId2 && h2h.Team2Id == teamId1)).ToList();
            if (headToheadStats.Count > 1)
            {
                repo.WriteLog(Database.SystemData.Severity.Error, "HeadToHeadStats has more then 1 record for teams", nameof(AnalysisDataRepository),
                                            "localhost", "[teamId1 = " + teamId1 + "][teamId2 = " + teamId2 + "]", "");
            }

            if (headToheadStats.Count == 0)
            {
                var newH2h = new HeadToHeadStats
                {
                    Team1Id = teamId1,
                    Team2Id = teamId2,                    
                    LastUpdate = DateTime.Now
                };

                Db.HeadToHeadStats.Add(newH2h);
                Db.SaveChanges();
                headToheadStats.Add(newH2h);

                
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to HeadToHeadStats table new record", nameof(AnalysisDataRepository),
                            "localhost", "[teamId1 = " + teamId1 + "][teamId2 = " + teamId2 + "]", "");
            }

            Db.Database.Connection.Close();
            return headToheadStats.First();
        }

        public void UpdateHeadToHeadStats(HeadToHeadStats stats)
        {
            OpenConnection();

            var h2hStats = Db.HeadToHeadStats.Where(s => s.Team1Id == stats.Team1Id && s.Team2Id == stats.Team2Id).FirstOrDefault();

            if (h2hStats != null)
            {
                h2hStats.Copy(stats);
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in HeadToHeadStats table", nameof(AnalysisDataRepository),
                            "localhost", "[teamId1 = " + h2hStats.Team1Id + "][teamId2 = " + h2hStats.Team2Id + "]", "");
                Db.SaveChanges();
            }

            Db.Database.Connection.Close();
        }        

        public void UpdateSeasonStats(SeasonStats stats)
        {
            OpenConnection();

            var seasonStats = Db.SeasonStats.Where(s => s.LeagueSeasonId == stats.LeagueSeasonId).FirstOrDefault();

            if (seasonStats != null)
            {
                seasonStats.Copy(stats);
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in SeasonStats table", nameof(AnalysisDataRepository),
                            "localhost", "[LeagueSeasonId = " + seasonStats.LeagueSeasonId + "]", "");
                Db.SaveChanges();
            }            

            Db.Database.Connection.Close();
        }

        private void OpenConnection()
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
        }
    }
}

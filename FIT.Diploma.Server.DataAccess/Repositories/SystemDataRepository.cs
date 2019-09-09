using FIT.Diploma.Server.Database.LeagueData;
using FIT.Diploma.Server.Database.SystemData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataAccess.Repositories
{
    public class SystemDataRepository : BaseRepository
    {
        //create new ResourceConfiguration (and save to DB) or if already exist get one
        public ResourceConfiguration GetResourceConfig(string domain)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var configs = (from n in Db.ResourceConfiguration where n.ResourceDomain == domain select n).ToList();
            if (configs.Count > 1) throw new Exception("DB contains duplicated ResourceConfiguration.");
            if (configs.Count == 0)
            {
                ResourceConfiguration newConfig = new ResourceConfiguration
                {
                    ResourceDomain = domain,
                    ResourceDataFormat = ResourceDataFormat.Html //by default
                };

                Db.ResourceConfiguration.Add(newConfig);
                Db.SaveChanges();

                Db.Database.Connection.Close();
                return GetResourceConfig(domain);
            }

            Db.Database.Connection.Close();
            return configs[0];
        }

        public bool AddResourceProcessingStatus(string targetUrl, ResourceConfiguration config)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            if ((from n in Db.ResourceProcessingStatus where n.TargetUrl == targetUrl select n).Any())
            {
                Console.WriteLine($"[AddResourceProcessingStatus] link({targetUrl}) already exist in DB");
                return false;
            }
            
            ResourceProcessingStatus newStatus = new ResourceProcessingStatus
            {
                ResourceConfiguration = config,
                ResourceConfigurationId = config.ResourceConfigurationId,
                Status = ProcessingStatus.Start,
                ProcessedMatches = 0,
                TargetUrl = targetUrl
            };

            Db.ResourceProcessingStatus.Add(newStatus);
            Db.SaveChanges();

            Db.Database.Connection.Close();
            return true;
        }

        public List<ResourceProcessingStatus> GetAllResources()
        {
            List<ResourceProcessingStatus> result = new List<ResourceProcessingStatus>();
            try
            {
                result = (from n in Db.ResourceProcessingStatus select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        //update properties of ResourceProcessingStatus in DB
        public void UpdateResource(ResourceProcessingStatus status)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var standingTableDb = (from n in Db.ResourceProcessingStatus where n.ResourceProcessingStatusId == status.ResourceProcessingStatusId select n).FirstOrDefault();
            if (standingTableDb != null)
            {
                standingTableDb.Status = status.Status;
                standingTableDb.ProcessedMatches = status.ProcessedMatches;
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in ResourceProcessingStatus table", nameof(SystemDataRepository),
                            "localhost", "[ResourceProcessingStatusId = " + status.ResourceProcessingStatusId + "]", "");
            }
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }

        public List<StandingTableAnalysis> GetStandingTables()
        {
            List<StandingTableAnalysis> result = new List<StandingTableAnalysis>();
            try
            {
                result = (from n in Db.StandingTableAnalysis select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        public List<StandingTableAnalysis_AnalyzedGames> GetStandingTableAnalyzedGames(int? leagueSeasonId = null)
        {
            var result = new List<StandingTableAnalysis_AnalyzedGames>();
            try
            {
                if(leagueSeasonId == null) result = (from n in Db.StandingTable_AnalyzedGames select n).ToList();
                else result = (from n in Db.StandingTable_AnalyzedGames where n.LeagueSeasonId == leagueSeasonId select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        public List<HeadToHeadStats_AnalyzedGames> GetHeadToHeadStatsAnalyzedGames(int? leagueSeasonId = null)
        {
            var result = new List<HeadToHeadStats_AnalyzedGames>();
            try
            {
                if (leagueSeasonId == null) result = (from n in Db.HeadToHeadStats_AnalyzedGames select n).ToList();
                else result = (from n in Db.HeadToHeadStats_AnalyzedGames where n.LeagueSeasonId == leagueSeasonId select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        public List<SeasonStats_AnalyzedGames> GetSeasonStatsAnalyzedGames(int? leagueSeasonId = null)
        {
            var result = new List<SeasonStats_AnalyzedGames>();
            try
            {
                if (leagueSeasonId == null) result = (from n in Db.SeasonStats_AnalyzedGames select n).ToList();
                else result = (from n in Db.SeasonStats_AnalyzedGames where n.LeagueSeasonId == leagueSeasonId select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        public List<SeasonRound_AnalyzedGames> GetSeasonRoundAnalyzedGames(int? leagueSeasonId = null)
        {
            var result = new List<SeasonRound_AnalyzedGames>();
            try
            {
                if (leagueSeasonId == null) result = (from n in Db.SeasonRound_AnalyzedGames select n).ToList();
                else result = (from n in Db.SeasonRound_AnalyzedGames where n.LeagueSeasonId == leagueSeasonId select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        public void DeleteRecord_StandingTable_AnalyzedGames(int gameId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var gameDb = (from n in Db.StandingTable_AnalyzedGames where n.GameId == gameId select n).FirstOrDefault();
            if (gameDb != null)
            {
                Db.StandingTable_AnalyzedGames.Attach(gameDb);
                Db.StandingTable_AnalyzedGames.Remove(gameDb);
                Db.SaveChanges();
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Delete record from StandingTable_AnalyzedGames table", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + gameId + "]", "");
            }

            Db.Database.Connection.Close();
        }

        public void AddAnalyzedGames_StandingTable(List<int> games, int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            foreach (var game in games)
            {
                var analyzedGame = new StandingTableAnalysis_AnalyzedGames
                {
                    GameId = game,
                    LeagueSeasonId = leagueSeasonId,
                    AnalysisFinished = true
                };

                Db.StandingTable_AnalyzedGames.Add(analyzedGame);
            }

            Db.SaveChanges();
            Db.Database.Connection.Close();
        }

        public List<AverageRoundStats_AnalyzedSeasons> GetAverageRoundStatsAnalyzedSeasons()
        {
            var result = new List<AverageRoundStats_AnalyzedSeasons>();
            try
            {
                result = (from n in Db.AverageRoundStats_AnalyzedSeasons select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        public List<TeamForm_AnalyzedSeasons> GetTeamFormAnalyzedSeasons()
        {
            var result = new List<TeamForm_AnalyzedSeasons>();
            try
            {
                result = (from n in Db.TeamForm_AnalyzedSeasons select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        public List<BookmakerOddsStats_AnalyzedGames> GetBookmakerOddsStatsAnalyzeGames(int? leagueSeasonId = null)
        {
            var result = new List<BookmakerOddsStats_AnalyzedGames>();
            try
            {
                if (leagueSeasonId == null) result = (from n in Db.BookmakerOddsStats_AnalyzedGames select n).ToList();
                else result = (from n in Db.BookmakerOddsStats_AnalyzedGames where n.LeagueSeasonId == leagueSeasonId select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        public void AddAnalyzedSeasons_AverageRoundStats(int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var analyzedSeason = new AverageRoundStats_AnalyzedSeasons
            {
                LeagueSeasonId = leagueSeasonId,
                AnalysisFinished = true
            };
            Db.AverageRoundStats_AnalyzedSeasons.Add(analyzedSeason);

            Db.SaveChanges();
            Db.Database.Connection.Close();
        }

        public void AddAnalyzedSeasons_TeamForm(int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var analyzedSeason = new TeamForm_AnalyzedSeasons
            {
                LeagueSeasonId = leagueSeasonId,
                AnalysisFinished = true
            };
            Db.TeamForm_AnalyzedSeasons.Add(analyzedSeason);

            Db.SaveChanges();
            Db.Database.Connection.Close();
        }
        
        public void AddAnalyzedGames_BookmakerOddsStats(List<int> games, int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            foreach (var game in games)
            {
                var analyzedGame = new BookmakerOddsStats_AnalyzedGames
                {
                    GameId = game,
                    LeagueSeasonId = leagueSeasonId,
                    AnalysisFinished = true
                };

                Db.BookmakerOddsStats_AnalyzedGames.Add(analyzedGame);
            }

            Db.SaveChanges();
            Db.Database.Connection.Close();
        }

        public void DeleteRecord_BookmakerOddsStats_AnalyzedGames(int gameId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var gameDb = (from n in Db.BookmakerOddsStats_AnalyzedGames where n.GameId == gameId select n).FirstOrDefault();
            if (gameDb != null)
            {
                Db.BookmakerOddsStats_AnalyzedGames.Attach(gameDb);
                Db.BookmakerOddsStats_AnalyzedGames.Remove(gameDb);
                Db.SaveChanges();
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Delete record from BookmakerOddsStats_AnalyzedGames table", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + gameId + "]", "");
            }

            Db.Database.Connection.Close();
        }


        public void AddAnalyzedGames_SeasonRounds(List<int> games, int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            foreach (var game in games)
            {
                var analyzedGame = new SeasonRound_AnalyzedGames
                {
                    GameId = game,
                    LeagueSeasonId = leagueSeasonId,
                    AnalysisFinished = true
                };

                Db.SeasonRound_AnalyzedGames.Add(analyzedGame);
            }

            Db.SaveChanges();
            Db.Database.Connection.Close();
        }

        public void DeleteRecord_SeasonRound_AnalyzedGames(int gameId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var gameDb = (from n in Db.SeasonRound_AnalyzedGames where n.GameId == gameId select n).FirstOrDefault();
            if (gameDb != null)
            {
                Db.SeasonRound_AnalyzedGames.Attach(gameDb);
                Db.SeasonRound_AnalyzedGames.Remove(gameDb);
                Db.SaveChanges();
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Delete record from SeasonRound_AnalyzedGames table", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + gameId + "]", "");
            }

            Db.Database.Connection.Close();
        }


        public void AddAnalyzedGames_SeasonStats(List<int> games, int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            foreach (var game in games)
            {
                var analyzedGame = new SeasonStats_AnalyzedGames
                {
                    GameId = game,
                    LeagueSeasonId = leagueSeasonId,
                    AnalysisFinished = true
                };

                Db.SeasonStats_AnalyzedGames.Add(analyzedGame);
            }

            Db.SaveChanges();
            Db.Database.Connection.Close();
        }

        public void DeleteRecord_SeasonStats_AnalyzedGames(int gameId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var gameDb = (from n in Db.SeasonStats_AnalyzedGames where n.GameId == gameId select n).FirstOrDefault();
            if (gameDb != null)
            {
                Db.SeasonStats_AnalyzedGames.Attach(gameDb);
                Db.SeasonStats_AnalyzedGames.Remove(gameDb);
                Db.SaveChanges();
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Delete record from SeasonStats_AnalyzedGames table", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + gameId + "]", "");
            }

            Db.Database.Connection.Close();
        }

        public void AddAnalyzedGames_HeadToHeadStats(List<int> games, int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
            var repo = new LogRepository();

            foreach (var game in games)
            {
                var analyzedGame = new HeadToHeadStats_AnalyzedGames
                {
                    GameId = game,
                    LeagueSeasonId = leagueSeasonId,
                    AnalysisFinished = true
                };

                Db.HeadToHeadStats_AnalyzedGames.Add(analyzedGame);
            }

            Db.SaveChanges();
            Db.Database.Connection.Close();
        }

        public void DeleteRecord_HeadToHeadStats_AnalyzedGames(int gameId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var gameDb = (from n in Db.HeadToHeadStats_AnalyzedGames where n.GameId == gameId select n).FirstOrDefault();
            if (gameDb != null)
            {
                Db.HeadToHeadStats_AnalyzedGames.Attach(gameDb);
                Db.HeadToHeadStats_AnalyzedGames.Remove(gameDb);
                Db.SaveChanges();
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Delete record from HeadToHeadStats_AnalyzedGames table", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + gameId + "]", "");
            }

            Db.Database.Connection.Close();
        }

        public StandingTableAnalysis GetStandingTable(LeagueSeason season)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var standingTable = (from n in Db.StandingTableAnalysis where n.LeagueSeasonId == season.LeagueSeasonId select n).FirstOrDefault();

            if (standingTable == null)
            {
                standingTable = new StandingTableAnalysis
                {
                    AnalysisDone = false,
                    Created = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    LeagueSeasonId = season.LeagueSeasonId
                };

                Db.StandingTableAnalysis.Add(standingTable);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Severity.Information, "Insert to StandingTableAnalysis table new record", nameof(SystemDataRepository),
                            "localhost", "[LeagueSeasonId = " + standingTable.LeagueSeasonId + "]", "");
            }

            Db.Database.Connection.Close();
            return standingTable;
        }

        //update properties of StandingTable in DB
        public void UpdateStandingTable(StandingTableAnalysis standingTable)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var standingTableDb = (from n in Db.StandingTableAnalysis where n.LeagueSeasonId == standingTable.LeagueSeasonId select n).FirstOrDefault();
            if (standingTableDb != null)
            {
                standingTableDb.Copy(standingTable);
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in StandingTableAnalysis table", nameof(SystemDataRepository),
                            "localhost", "[LeagueSeasonId = " + standingTable.LeagueSeasonId + "]", "");
            }
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }
    }
}

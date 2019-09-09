using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataAccess.Repositories
{
    public class LeagueDataRepository : BaseRepository, ILeagueDataRepository
    {
        private const int ROUND_GAMES_NUMBER = 10;
        //default location for all clubs in LaLiga (could be changed after)
        public Location GetSpainLocation()
        {
            return (from n in Db.Locations where n.Country == "Spain" && n.City == null select n).FirstOrDefault();
        }

        //get LaLiga object
        public FootballLeague GetLaLiga()
        {
            return (from n in Db.FootballLeagues where n.Name == "Spain LaLiga" select n).FirstOrDefault();
        }

        public List<FootballLeague> GetAllLeagues()
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var allLeagues = (from n in Db.FootballLeagues select n).ToList();

            Db.Database.Connection.Close();
            return allLeagues;
        }

        //create new season with fake start and end dates (shoul be set later automaticcally by analyzers)
        public LeagueSeason CreateAndSaveLeagueSeason(int startYear, FootballLeague league)
        {
            if(Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
             
            var newSeason = (from n in Db.LeagueSeasons where n.StartYear == startYear && n.League.LeagueId == league.LeagueId select n).FirstOrDefault();

            if(newSeason == null)
            {
                newSeason = new LeagueSeason
                {
                    StartYear = startYear,
                    League = league,
                    StartDate = new DateTime(startYear, 1, 1),
                    EndDate = new DateTime(startYear + 1, 1, 1)
                };

                Db.LeagueSeasons.Add(newSeason);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to LeagueSeasons table new record", nameof(LeagueDataRepository),
                            "localhost", "[LeagueId = " + league.LeagueId + "] [StartYear = " + newSeason.StartYear + "]", "");
            }           

            Db.Database.Connection.Close();
            return newSeason;
        }

        //get league season
        public LeagueSeason GetLeagueSeasonById(int seasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
            var result = (from n in Db.LeagueSeasons where n.LeagueSeasonId == seasonId select n).FirstOrDefault();
            Db.Database.Connection.Close();
            return result;
        }

        //get all league seasons
        public List<LeagueSeason> GetAllLeagueSeasons(int leagueId)
        {
            List<LeagueSeason> result = new List<LeagueSeason>();
            try
            {
                result = (from n in Db.LeagueSeasons where n.League.LeagueId == leagueId select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        //get all league seasons
        public List<LeagueSeason> GetAllLeagueSeasons(FootballLeague league)
        {
            return GetAllLeagueSeasons(league.LeagueId);
        }

        //update properties of leagueSeason in DB
        public void UpdateLeagueSeason(LeagueSeason leagueSeason)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var league = (from n in Db.LeagueSeasons where n.LeagueSeasonId == leagueSeason.LeagueSeasonId select n).FirstOrDefault();
            if(league != null)
            {
                league.Copy(leagueSeason);
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in LeagueSeasons table", nameof(LeagueDataRepository),
                            "localhost", "[LeagueSeasonId = " + league.LeagueSeasonId + "]", "");
            }
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }

        //get current round of season - create one or get latest (if latest has already ROUND_GAMES_NUMBER games - create new one)
        private SeasonRound GetLatestSeasonRound(LeagueSeason leagueSeason)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var round = (from n in Db.SeasonRounds where n.LeagueSeasonId == leagueSeason.LeagueSeasonId orderby n.RoundId descending select n).FirstOrDefault();

            if(round == null) //leagueSeason doesn't have rounds yet
            {
                round = new SeasonRound
                {
                    LeagueSeason = leagueSeason,
                    LeagueSeasonId = leagueSeason.LeagueSeasonId,
                    IsFinished = false,
                    RoundNumber = 1,
                    GamePlayed = 0,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    LastUpdate = DateTime.Now
                };

                Db.SeasonRounds.Add(round);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to SeasonRounds table new record", nameof(LeagueDataRepository),
                            "localhost", "[RoundId = " + round.RoundId + "]", "");
            }
            else if(round.GamePlayed >= ROUND_GAMES_NUMBER) //round already has maximum number of games -> should create new round
            {
                var newRound = new SeasonRound
                {
                    LeagueSeason = leagueSeason,
                    LeagueSeasonId = leagueSeason.LeagueSeasonId,
                    IsFinished = false,
                    RoundNumber = round.RoundNumber + 1,
                    GamePlayed = 0,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    LastUpdate = DateTime.Now
                };

                Db.SeasonRounds.Add(newRound);
                Db.SaveChanges();

                round = newRound;

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to SeasonRounds table new record", nameof(LeagueDataRepository),
                            "localhost", "[RoundId = " + round.RoundId + "]", "");
            }            

            Db.Database.Connection.Close();
            return round;
        }

        public List<FootballTeam> GetFootballTeamsByIds(int [] teamIds)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var footballTeams = new List<FootballTeam>();
            footballTeams = (from n in Db.FootballTeams where teamIds.Contains(n.TeamId) select n).ToList();

            Db.Database.Connection.Close();
            return footballTeams;
        }

        //get football team by name or create one if doesn't exist
        public FootballTeam GetFootballTeam(string name, LeagueSeason season, string sourceId = "")
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var footballTeam = (from n in Db.FootballTeams where n.Name == name select n).FirstOrDefault();
            if (footballTeam?.ParentTeamId != null)
            {
                footballTeam = (from n in Db.FootballTeams where n.TeamId == footballTeam.ParentTeamId select n).FirstOrDefault();
            }

            if (footballTeam == null)
            {
                footballTeam = new FootballTeam
                {
                    Location = season.League.Location,
                    LocationId = season.League.Location.LocationId,
                    Name = name,
                    SourceId = sourceId
                };

                Db.FootballTeams.Add(footballTeam);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to FootballTeams table new record", nameof(LeagueDataRepository),
                            "localhost", "[FootballTeamId = " + footballTeam.TeamId + "]", "");
            }

            Db.Database.Connection.Close();
            return footballTeam;
        }

        public bool IsGameExist(FootballTeam homeTeam, FootballTeam awayTeam, DateTime matchDate)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
            bool result = (from n in Db.Games
                           where
                               n.HomeTeamId == homeTeam.TeamId &&
                               n.AwayTeamId == awayTeam.TeamId &&
                               n.Date == matchDate
                           select n).Any();
            Db.Database.Connection.Close();
            return result;
        }
        
        //Get game or create new
        public Game GetGame(FootballTeam homeTeam, FootballTeam awayTeam, DateTime matchDate, LeagueSeason leagueSeason, out bool onlyCreated)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
            onlyCreated = false;

            var game = (from n in Db.Games
                        where 
                            n.HomeTeamId == homeTeam.TeamId &&
                            n.AwayTeamId == awayTeam.TeamId &&
                            DbFunctions.TruncateTime(n.Date) == matchDate.Date
                        select n).FirstOrDefault();

            if (game == null)
            {
                var round = GetLatestSeasonRound(leagueSeason);
                game = new Game
                {
                    Date = matchDate,
                    AwayTeamId = awayTeam.TeamId,
                    HomeTeamId = homeTeam.TeamId,
                    AwayFootballTeam = awayTeam,
                    HomeFootballTeam = homeTeam,
                    RoundId = round.RoundId,
                    SeasonRound = round                    
                };

                Db.Games.Add(game);
                Db.SaveChanges();
                onlyCreated = true;

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to Games table new record", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + game.GameId + "]", "");
            }

            Db.Database.Connection.Close();
            return game;
        }

        public Game GetGame(string homeTeamName, string awayTeamName, DateTime matchDate, out bool onlyCreated, string source = "")
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed)  Db.Database.Connection.Open();
            onlyCreated = false;
            var year = matchDate.Year;

            //temp hack
            if (source == "bwin")
            {
                if (matchDate.Month <= 7) year--;
            }

            var season = CreateAndSaveLeagueSeason(year, GetLaLiga());
            var round = GetLatestSeasonRound(season);
            var homeTeam = GetFootballTeam(homeTeamName, season, source);
            var awayTeam = GetFootballTeam(awayTeamName, season, source);

            var game = (from n in Db.Games
                        where
                            n.HomeTeamId == homeTeam.TeamId &&
                            n.AwayTeamId == awayTeam.TeamId &&
                            n.Date == matchDate
                        select n).FirstOrDefault();

            if (game == null)
            {
                game = new Game
                {
                    Date = matchDate,
                    AwayTeamId = awayTeam.TeamId,
                    HomeTeamId = homeTeam.TeamId,
                    AwayFootballTeam = awayTeam,
                    HomeFootballTeam = homeTeam,
                    RoundId = round.RoundId,
                    SeasonRound = round
                };

                Db.Games.Add(game);
                Db.SaveChanges();
                onlyCreated = true;

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to Games table new record", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + game.GameId + "]", "");
            }

            Db.Database.Connection.Close();
            return game;
        }

        public List<Game> GetAllSeasonGames(int seasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            List<Game> games = new List<Game>();
            try
            {
                games = (from n in Db.Games where n.SeasonRound.LeagueSeasonId == seasonId select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
            }

            Db.Database.Connection.Close();
            return games;
        }

        public List<Game> GetAllGamesInTimeRange(DateTime fromDate, DateTime toDate, int leagueId = -1)
        {
            if (leagueId == -1)
                leagueId = GetLaLiga().LeagueId;

            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            List<Game> games = new List<Game>();
            try
            {
                games = (from g in Db.Games
                            join r in Db.SeasonRounds on g.RoundId equals r.RoundId
                            join l in Db.LeagueSeasons on r.LeagueSeasonId equals l.LeagueSeasonId
                            where
                                l.League.LeagueId == leagueId
                                && g.Date > fromDate 
                                && g.Date < toDate
                            orderby g.Date
                            select g).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
            }

            Db.Database.Connection.Close();
            return games;
        }

        //Update game record
        public void UpdateGameRecord(Game game)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var gameDb = (from n in Db.Games where n.GameId == game.GameId select n).FirstOrDefault();
            if (gameDb != null)
            {
                gameDb.Copy(game);
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in Games table", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + gameDb.GameId + "]", "");
            }
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }

        public void DeleteGameStats(int gameId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var gameDb = (from n in Db.GameStats where n.GameId == gameId select n).FirstOrDefault();
            if (gameDb != null)
            {
                Db.GameStats.Attach(gameDb);
                Db.GameStats.Remove(gameDb);
                Db.SaveChanges();
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Delete record from GameStats table", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + gameId + "]", "");
            }

            Db.Database.Connection.Close();
        }

        //get game stats or create one
        public GameStats GetGameStats(int gameId, out bool onlyCreated)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
            onlyCreated = false;

            var gameStats = (from n in Db.GameStats where n.GameId == gameId select n).FirstOrDefault();

            if (gameStats == null)
            {
                gameStats = new GameStats
                {
                    GameId = gameId                    
                };

                Db.GameStats.Add(gameStats);
                Db.SaveChanges();
                onlyCreated = true;

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to GameStats table new record", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + gameStats.GameId + "]", "");
            }

            Db.Database.Connection.Close();
            return gameStats;
        }

        //Update GameStats record
        public void UpdateGameStats(GameStats gameStats)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var gameStatsDb = (from n in Db.GameStats where n.GameId == gameStats.GameId select n).FirstOrDefault();
            if (gameStatsDb != null)
            {
                gameStatsDb.Copy(gameStats);
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in GameStats table", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + gameStatsDb.GameId + "]", "");
            }
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }

        public DateTime? GetNextUpcomingGameDateTime(int leagueId = -1)
        {
            if (leagueId == -1)
                leagueId = GetLaLiga().LeagueId;

            DateTime? result = null;

            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();
            
            var game = (from g in Db.Games
                        join r in Db.SeasonRounds on g.RoundId equals r.RoundId
                        join l in Db.LeagueSeasons on r.LeagueSeasonId equals l.LeagueSeasonId
                        where 
                            l.League.LeagueId == leagueId
                            && g.Date > DateTime.Now
                        orderby g.Date
                        select g).FirstOrDefault();

            if (game != null)
                result = game.Date;

            Db.Database.Connection.Close();
            return result;
        }

        public int GetSeasonRoundCount(int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var result = (from n in Db.SeasonRounds where n.LeagueSeasonId == leagueSeasonId select n).Count();            

            Db.Database.Connection.Close();
            return result;
        }

        public List<SeasonRound> GetCurrentSeasonRounds(int leagueSeasonId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var rounds = (from n in Db.SeasonRounds where n.LeagueSeasonId == leagueSeasonId select n).ToList();           

            Db.Database.Connection.Close();
            return rounds;
        }

        public List<Game> GetAllRoundGames(int roundId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            List<Game> games = new List<Game>();
            try
            {
                games = (from n in Db.Games where n.SeasonRound.RoundId == roundId select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
            }

            Db.Database.Connection.Close();
            return games;
        }

        public void UpdateSeasonRound(SeasonRound round)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var roundDb = (from n in Db.SeasonRounds where n.RoundId == round.RoundId select n).FirstOrDefault();
            if (roundDb != null)
            {
                roundDb.Copy(round);
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in SeasonRounds table", nameof(LeagueDataRepository),
                            "localhost", "[RoundId = " + roundDb.RoundId + "]", "");
            }
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }

        public void RemoveSeasonRound(int roundId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var roundDb = (from n in Db.SeasonRounds where n.RoundId == roundId select n).FirstOrDefault();
            if (roundDb != null)
            {
                Db.SeasonRounds.Attach(roundDb);
                Db.SeasonRounds.Remove(roundDb);
                Db.SaveChanges();
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Remove record from SeasonRounds table", nameof(LeagueDataRepository),
                            "localhost", "[RoundId = " + roundId + "]", "");                
            }
            

            Db.Database.Connection.Close();
        }

        public List<Game> GetAllUpcomingGames(int leagueId = -1)
        {
            if (leagueId == -1)
                leagueId = GetLaLiga().LeagueId;

            var result = new List<Game>();

            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            result = (from g in Db.Games
                        join r in Db.SeasonRounds on g.RoundId equals r.RoundId
                        join l in Db.LeagueSeasons on r.LeagueSeasonId equals l.LeagueSeasonId
                        where
                            l.League.LeagueId == leagueId
                            && g.Date > DateTime.Now
                        orderby g.Date
                        select g).ToList();

            Db.Database.Connection.Close();
            return result;
        }

        public List<Game> GetTeamsHistory(int teamId1, int teamId2)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            List<Game> games = new List<Game>();
            try
            {
                games = (from n in Db.Games where 
                         (n.HomeTeamId == teamId1 && n.AwayTeamId == teamId2)
                         ||
                         (n.HomeTeamId == teamId2 && n.AwayTeamId == teamId1)
                         select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
            }

            Db.Database.Connection.Close();
            return games;
        }

        public SeasonRound GetCurrentSeasonRound(LeagueSeason leagueSeason)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var rounds = (from n in Db.SeasonRounds where n.LeagueSeasonId == leagueSeason.LeagueSeasonId orderby n.RoundId descending select n).ToList();

            if (rounds.Count == 0)
                return GetLatestSeasonRound(leagueSeason);

            var notFinishedRounds = rounds.Where(r => !r.IsFinished).OrderBy(r => r.RoundId).ToList();
            if (notFinishedRounds.Count == 0)
                return rounds.First();

            Db.Database.Connection.Close();
            return notFinishedRounds.First();
        }

        public void DeleteGameRecord(int gameId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var gameDb = (from n in Db.Games where n.GameId == gameId select n).FirstOrDefault();
            if (gameDb != null)
            {
                Db.Games.Attach(gameDb);
                Db.Games.Remove(gameDb);
                Db.SaveChanges();
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Delete record from Games table", nameof(LeagueDataRepository),
                            "localhost", "[GameId = " + gameId + "]", "");
            }           

            Db.Database.Connection.Close();
        }
    }
}

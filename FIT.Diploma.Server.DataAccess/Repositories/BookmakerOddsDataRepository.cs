using FIT.Diploma.Server.Database.BookmakerOddsData;
using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataAccess.Repositories
{
    public class BookmakerOddsDataRepository : BaseRepository
    {
        public Bookmaker GetBookmaker(string bookmakerName)
        {
            Db.Database.Connection.Open();

            var bookmaker = (from n in Db.Bookmakers where n.BookmakerName == bookmakerName select n).FirstOrDefault();

            if (bookmaker == null)
            {
                bookmaker = new Bookmaker
                {
                    BookmakerName = bookmakerName
                };

                Db.Bookmakers.Add(bookmaker);
                Db.SaveChanges();

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to Bookmakers table new record", nameof(BookmakerOddsDataRepository),
                            "localhost", "[bookmakerId = " + bookmaker.BookmakerId + "] [bookmakerName = " + bookmaker.BookmakerName + "]", "");
            }

            Db.Database.Connection.Close();
            return bookmaker;
        }

        public List<BookmakerOdds> GetAllBookmakerOddsForGame(int gameId)
        {
            Db.Database.Connection.Open();
            List<BookmakerOdds> result = new List<BookmakerOdds>();

            try
            {
                result = (from n in Db.BookmakerOdds where n.GameId == gameId select n).ToList();
            }
            catch(Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            Db.Database.Connection.Close();
            return result;
        }

        public BookmakerOdds GetLatestBookmakerOddsForGame(int gameId)
        {
            Db.Database.Connection.Open();
            BookmakerOdds result = null;

            try
            {
                result = (from n in Db.BookmakerOdds where n.GameId == gameId orderby n.Game.Date descending select n).FirstOrDefault();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            Db.Database.Connection.Close();
            return result;
        }

        private bool IsGameFinished(Game game)
        {
            var date = game.Date;
            //if game doesn't have right date (we have only 20 (maximum) last years)
            if (DateTime.Now.AddYears(-100) > date) return false;
            return DateTime.Now > date;
        }

        //get game stats or create one
        public BookmakerOdds GetBookmakerOdds(int gameId, int bookmakerId, out bool onlyCreated)
        {
            Db.Database.Connection.Open();
            onlyCreated = false;
            var repo = new LogRepository();

            var bookmakerOdds = (from n in Db.BookmakerOdds where n.GameId == gameId && n.BookmakerId == bookmakerId select n).FirstOrDefault();

            //if bookmaker odds already exist on this game but it's more then 10 minutes old -> create new record
            if (bookmakerOdds != null)
            {
                if (DateTime.Now.AddMinutes(-5) > bookmakerOdds.CreatedTime && !IsGameFinished(bookmakerOdds.Game))
                {
                    repo.WriteLog(Database.SystemData.Severity.Verbose, "BookmakerOdds table has old record for this game.", nameof(BookmakerOddsDataRepository),
                            "localhost", "[GameId = " + bookmakerOdds.GameId + "] [BookmakerId = " + bookmakerOdds.BookmakerId + "]", "");
                    bookmakerOdds = null;
                }                    
            }

            if (bookmakerOdds == null)
            {
                bookmakerOdds = new BookmakerOdds
                {
                    GameId = gameId,
                    BookmakerId = bookmakerId,
                    CreatedTime = DateTime.Now
                };

                Db.BookmakerOdds.Add(bookmakerOdds);
                Db.SaveChanges();
                onlyCreated = true;
                
                repo.WriteLog(Database.SystemData.Severity.Information, "Insert to BookmakerOdds table new record", nameof(BookmakerOddsDataRepository),
                            "localhost", "[GameId = " + bookmakerOdds.GameId + "] [BookmakerId = " + bookmakerOdds.BookmakerId + "]", "");
            }

            Db.Database.Connection.Close();
            return bookmakerOdds;
        }

        public void UpdateGameIdForBookmakerOdds(int oldGameId, int newGameId)
        {
            if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open();

            var results = (from n in Db.BookmakerOdds
                           where n.GameId == oldGameId
                           select n).ToList();

            foreach (var odd in results)
            {
                odd.GameId = newGameId;

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update GameID in BookmakerOdds table", nameof(AnalysisDataRepository),
                            "localhost", "[OldGameId = " + oldGameId + "] [NewGameId" + newGameId + "]", "");
            }
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }

        //Update BookmakerOdds record
        public void UpdateBookmakerOdds(BookmakerOdds bookmakerOdds)
        {
            Db.Database.Connection.Open();

            var bookmakerOddsDb = (from n in Db.BookmakerOdds
                                   where n.GameId == bookmakerOdds.GameId && n.BookmakerId == bookmakerOdds.BookmakerId
                                   select n).FirstOrDefault();
            if (bookmakerOddsDb != null)
            {
                bookmakerOddsDb.Copy(bookmakerOdds);

                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Information, "Update record in BookmakerOdds table", nameof(BookmakerOddsDataRepository),
                            "localhost", "[GameId = " + bookmakerOdds.GameId + "] [BookmakerId = " + bookmakerOdds.BookmakerId + "]", "");
            }
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }
    }
}

using System;
using FIT.Diploma.Shared.Database;

namespace FIT.Diploma.Shared.DataAccess
{
    public class BaseRepository : IDisposable
    {
        private ServerDbContext db;

        public BaseRepository() { }

        private bool internalDbContext = false;
        public BaseRepository(ServerDbContext _db)
        {
            db = _db;
        }


        protected ServerDbContext Db
        {
            get
            {
                if (db == null)
                {
                    db = new ServerDbContext();
                    internalDbContext = true;
                }
                return db;
            }
        }


        public void Dispose()
        {
            //if (db != null && internalDbContext) db.Dispose();
        }

        ~BaseRepository()
        {
            Dispose();
        }
    }
}

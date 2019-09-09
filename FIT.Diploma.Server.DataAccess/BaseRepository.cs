using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIT.Diploma.Server.Database;

namespace FIT.Diploma.Server.DataAccess
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

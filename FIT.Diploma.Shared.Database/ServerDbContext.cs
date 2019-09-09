using FIT.Diploma.Shared.Database.Models;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FIT.Diploma.Shared.Database
{
    public class ServerDbContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        public ServerDbContext()
            : base(ConfigurationManager.ConnectionStrings["DiplomaDB"].ConnectionString)
        {
            this.Database.CommandTimeout = 180;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }   
}

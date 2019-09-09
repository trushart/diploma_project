using System.Data.Entity;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using FIT.Diploma.Server.Database.LeagueData;
using FIT.Diploma.Server.Database.BookmakerOddsData;
using FIT.Diploma.Server.Database.AnalyzerResults;
using FIT.Diploma.Server.Database.SystemData;
using FIT.Diploma.Server.Database.SchedulerData;

namespace FIT.Diploma.Server.Database
{
    public class ServerDbContext : DbContext
    {
        //Database LeagueData
        public DbSet<FootballLeague> FootballLeagues { get; set; }
        public DbSet<FootballTeam> FootballTeams { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameRosourceId> GameRosourceIds { get; set; }
        public DbSet<GameStats> GameStats { get; set; }
        public DbSet<LeagueSeason> LeagueSeasons { get; set; }
        public DbSet<LeagueSeasonReferees> LeagueSeasonReferees { get; set; }
        public DbSet<LeagueSeasonTeams> LeagueSeasonTeams { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<SeasonRound> SeasonRounds { get; set; }

        //Database BookmakerOddsData
        public DbSet<Bookmaker> Bookmakers { get; set; }
        public DbSet<BookmakerOdds> BookmakerOdds { get; set; }

        //Database AnalyzerResults
        public DbSet<AverageRoundStats> AverageRoundStats { get; set; }
        public DbSet<BookmakerOddsStats> BookmakerOddsStats { get; set; }
        public DbSet<FootballTeamForm> FootballTeamForm { get; set; }
        public DbSet<HeadToHeadStats> HeadToHeadStats { get; set; }
        public DbSet<SeasonStats> SeasonStats { get; set; }
        public DbSet<Prediction> Predictions { get; set; }

        //Database SystemData
        public DbSet<ResourceConfiguration> ResourceConfiguration { get; set; }
        public DbSet<ResourceProcessingStatus> ResourceProcessingStatus { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<StandingTableAnalysis> StandingTableAnalysis { get; set; }
        public DbSet<StandingTableAnalysis_AnalyzedGames> StandingTable_AnalyzedGames { get; set; }
        public DbSet<SeasonStats_AnalyzedGames> SeasonStats_AnalyzedGames { get; set; }
        public DbSet<SeasonRound_AnalyzedGames> SeasonRound_AnalyzedGames { get; set; }
        public DbSet<HeadToHeadStats_AnalyzedGames> HeadToHeadStats_AnalyzedGames { get; set; }
        public DbSet<TeamForm_AnalyzedSeasons> TeamForm_AnalyzedSeasons { get; set; }
        public DbSet<BookmakerOddsStats_AnalyzedGames> BookmakerOddsStats_AnalyzedGames { get; set; }
        public DbSet<AverageRoundStats_AnalyzedSeasons> AverageRoundStats_AnalyzedSeasons { get; set; }

        //Database SchedulerData
        public DbSet<SchedulerTask> SchedulerTask { get; set; }
        public DbSet<TaskExecutor> TaskExecutor { get; set; }
        public DbSet<TriggerRule> TriggerRule { get; set; }

        public ServerDbContext() 
            : base(ConfigurationManager.ConnectionStrings["DiplomaDB"].ConnectionString)
            //: base(ConfigurationManager.ConnectionStrings["DiplomaDB_New"].ConnectionString)
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

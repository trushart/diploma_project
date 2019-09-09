namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStandingTableAnalysis_AnalyzedGames : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StandingTableAnalysis_AnalyzedGames",
                c => new
                    {
                        FootballTeamId = c.Int(nullable: false),
                        LeagueSeasonId = c.Int(nullable: false),
                        AnalysisFinished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.FootballTeamId, t.LeagueSeasonId })
                .ForeignKey("dbo.FootballTeam", t => t.FootballTeamId, cascadeDelete: true)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId, cascadeDelete: true)
                .Index(t => t.FootballTeamId)
                .Index(t => t.LeagueSeasonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StandingTableAnalysis_AnalyzedGames", "LeagueSeasonId", "dbo.LeagueSeason");
            DropForeignKey("dbo.StandingTableAnalysis_AnalyzedGames", "FootballTeamId", "dbo.FootballTeam");
            DropIndex("dbo.StandingTableAnalysis_AnalyzedGames", new[] { "LeagueSeasonId" });
            DropIndex("dbo.StandingTableAnalysis_AnalyzedGames", new[] { "FootballTeamId" });
            DropTable("dbo.StandingTableAnalysis_AnalyzedGames");
        }
    }
}

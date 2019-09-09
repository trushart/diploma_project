namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStandingTableAnalysis : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StandingTableAnalysis",
                c => new
                    {
                        LeagueSeasonId = c.Int(nullable: false),
                        AnalysisDone = c.Boolean(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.LeagueSeasonId)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId)
                .Index(t => t.LeagueSeasonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StandingTableAnalysis", "LeagueSeasonId", "dbo.LeagueSeason");
            DropIndex("dbo.StandingTableAnalysis", new[] { "LeagueSeasonId" });
            DropTable("dbo.StandingTableAnalysis");
        }
    }
}

namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddH2HStats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HeadToHeadStats_AnalyzedGames",
                c => new
                    {
                        GameId = c.Int(nullable: false),
                        LeagueSeasonId = c.Int(nullable: false),
                        AnalysisFinished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GameId)
                .ForeignKey("dbo.Game", t => t.GameId)
                .Index(t => t.GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HeadToHeadStats_AnalyzedGames", "GameId", "dbo.Game");
            DropIndex("dbo.HeadToHeadStats_AnalyzedGames", new[] { "GameId" });
            DropTable("dbo.HeadToHeadStats_AnalyzedGames");
        }
    }
}

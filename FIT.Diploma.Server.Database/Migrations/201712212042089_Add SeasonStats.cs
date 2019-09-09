namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeasonStats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeasonStats_AnalyzedGames",
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
            DropForeignKey("dbo.SeasonStats_AnalyzedGames", "GameId", "dbo.Game");
            DropIndex("dbo.SeasonStats_AnalyzedGames", new[] { "GameId" });
            DropTable("dbo.SeasonStats_AnalyzedGames");
        }
    }
}

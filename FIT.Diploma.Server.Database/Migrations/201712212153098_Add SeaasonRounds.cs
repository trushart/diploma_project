namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeaasonRounds : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeasonRound_AnalyzedGames",
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
            DropForeignKey("dbo.SeasonRound_AnalyzedGames", "GameId", "dbo.Game");
            DropIndex("dbo.SeasonRound_AnalyzedGames", new[] { "GameId" });
            DropTable("dbo.SeasonRound_AnalyzedGames");
        }
    }
}

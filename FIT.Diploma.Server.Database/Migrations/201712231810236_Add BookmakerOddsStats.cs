namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookmakerOddsStats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookmakerOddsStats_AnalyzedGames",
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
            DropForeignKey("dbo.BookmakerOddsStats_AnalyzedGames", "GameId", "dbo.Game");
            DropIndex("dbo.BookmakerOddsStats_AnalyzedGames", new[] { "GameId" });
            DropTable("dbo.BookmakerOddsStats_AnalyzedGames");
        }
    }
}

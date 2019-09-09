namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAverageRoundStats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AverageRoundStats_AnalyzedSeasons",
                c => new
                    {
                        LeagueSeasonId = c.Int(nullable: false),
                        AnalysisFinished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LeagueSeasonId)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId)
                .Index(t => t.LeagueSeasonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AverageRoundStats_AnalyzedSeasons", "LeagueSeasonId", "dbo.LeagueSeason");
            DropIndex("dbo.AverageRoundStats_AnalyzedSeasons", new[] { "LeagueSeasonId" });
            DropTable("dbo.AverageRoundStats_AnalyzedSeasons");
        }
    }
}

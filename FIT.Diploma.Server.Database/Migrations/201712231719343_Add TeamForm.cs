namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeamForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamForm_AnalyzedSeasons",
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
            DropForeignKey("dbo.TeamForm_AnalyzedSeasons", "LeagueSeasonId", "dbo.LeagueSeason");
            DropIndex("dbo.TeamForm_AnalyzedSeasons", new[] { "LeagueSeasonId" });
            DropTable("dbo.TeamForm_AnalyzedSeasons");
        }
    }
}

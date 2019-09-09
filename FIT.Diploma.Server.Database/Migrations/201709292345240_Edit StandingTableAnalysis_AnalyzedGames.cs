namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditStandingTableAnalysis_AnalyzedGames : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StandingTableAnalysis_AnalyzedGames", "FootballTeamId", "dbo.FootballTeam");
            DropForeignKey("dbo.StandingTableAnalysis_AnalyzedGames", "LeagueSeasonId", "dbo.LeagueSeason");
            DropIndex("dbo.StandingTableAnalysis_AnalyzedGames", new[] { "FootballTeamId" });
            DropIndex("dbo.StandingTableAnalysis_AnalyzedGames", new[] { "LeagueSeasonId" });
            DropPrimaryKey("dbo.StandingTableAnalysis_AnalyzedGames");
            AddColumn("dbo.StandingTableAnalysis_AnalyzedGames", "GameId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.StandingTableAnalysis_AnalyzedGames", "GameId");
            CreateIndex("dbo.StandingTableAnalysis_AnalyzedGames", "GameId");
            AddForeignKey("dbo.StandingTableAnalysis_AnalyzedGames", "GameId", "dbo.Game", "GameId");
            DropColumn("dbo.StandingTableAnalysis_AnalyzedGames", "FootballTeamId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StandingTableAnalysis_AnalyzedGames", "FootballTeamId", c => c.Int(nullable: false));
            DropForeignKey("dbo.StandingTableAnalysis_AnalyzedGames", "GameId", "dbo.Game");
            DropIndex("dbo.StandingTableAnalysis_AnalyzedGames", new[] { "GameId" });
            DropPrimaryKey("dbo.StandingTableAnalysis_AnalyzedGames");
            DropColumn("dbo.StandingTableAnalysis_AnalyzedGames", "GameId");
            AddPrimaryKey("dbo.StandingTableAnalysis_AnalyzedGames", new[] { "FootballTeamId", "LeagueSeasonId" });
            CreateIndex("dbo.StandingTableAnalysis_AnalyzedGames", "LeagueSeasonId");
            CreateIndex("dbo.StandingTableAnalysis_AnalyzedGames", "FootballTeamId");
            AddForeignKey("dbo.StandingTableAnalysis_AnalyzedGames", "LeagueSeasonId", "dbo.LeagueSeason", "LeagueSeasonId", cascadeDelete: true);
            AddForeignKey("dbo.StandingTableAnalysis_AnalyzedGames", "FootballTeamId", "dbo.FootballTeam", "TeamId", cascadeDelete: true);
        }
    }
}

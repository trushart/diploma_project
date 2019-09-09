namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedLeagueSeasonTeam : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.LeagueSeasonTeams", name: "CustomerId", newName: "FootballTeamId");
            RenameIndex(table: "dbo.LeagueSeasonTeams", name: "IX_CustomerId", newName: "IX_FootballTeamId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.LeagueSeasonTeams", name: "IX_FootballTeamId", newName: "IX_CustomerId");
            RenameColumn(table: "dbo.LeagueSeasonTeams", name: "FootballTeamId", newName: "CustomerId");
        }
    }
}

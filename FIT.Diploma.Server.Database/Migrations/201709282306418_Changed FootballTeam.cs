namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedFootballTeam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FootballTeam", "Data", c => c.String());
            AddColumn("dbo.FootballTeam", "ParentTeamId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FootballTeam", "ParentTeamId");
            DropColumn("dbo.FootballTeam", "Data");
        }
    }
}

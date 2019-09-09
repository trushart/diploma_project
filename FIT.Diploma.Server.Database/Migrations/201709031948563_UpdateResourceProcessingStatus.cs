namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateResourceProcessingStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResourceProcessingStatus", "TargetUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResourceProcessingStatus", "TargetUrl");
        }
    }
}

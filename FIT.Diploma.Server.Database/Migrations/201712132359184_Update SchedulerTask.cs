namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchedulerTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SchedulerTask", "FailReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SchedulerTask", "FailReason");
        }
    }
}

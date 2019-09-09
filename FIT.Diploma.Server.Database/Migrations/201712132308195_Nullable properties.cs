namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nullableproperties : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SchedulerTask", "Result", c => c.Int());
            AlterColumn("dbo.SchedulerTask", "UpdateDb", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SchedulerTask", "UpdateDb", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SchedulerTask", "Result", c => c.Int(nullable: false));
        }
    }
}

namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nullableproperties2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SchedulerTask", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SchedulerTask", "EndTime", c => c.DateTime(nullable: false));
        }
    }
}

namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchedulerTask1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SchedulerTask", "PlanningTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SchedulerTask", "StartTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SchedulerTask", "StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.SchedulerTask", "PlanningTime");
        }
    }
}

namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addschedulerdatamodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SchedulerTask",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        TaskExecutorId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Result = c.Int(nullable: false),
                        UpdateDb = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.TaskExecutor", t => t.TaskExecutorId, cascadeDelete: true)
                .Index(t => t.TaskExecutorId);
            
            CreateTable(
                "dbo.TaskExecutor",
                c => new
                    {
                        TaskExecutorId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CodeClass = c.String(),
                        Description = c.String(),
                        TimeOut = c.Int(nullable: false),
                        MaxRetries = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskExecutorId);
            
            CreateTable(
                "dbo.TriggerRule",
                c => new
                    {
                        RuleId = c.Int(nullable: false, identity: true),
                        FinishedExecutorId = c.Int(nullable: false),
                        ToStartExecutorId = c.Int(nullable: false),
                        DelayTime = c.Int(nullable: false),
                        CheckConfigs = c.Boolean(nullable: false),
                        WhenFinishedExecutorUpdateDb = c.Boolean(),
                    })
                .PrimaryKey(t => t.RuleId)
                .ForeignKey("dbo.TaskExecutor", t => t.FinishedExecutorId, cascadeDelete: false)
                .ForeignKey("dbo.TaskExecutor", t => t.ToStartExecutorId, cascadeDelete: false)
                .Index(t => t.FinishedExecutorId)
                .Index(t => t.ToStartExecutorId);
            
            AddColumn("dbo.FootballLeague", "Description", c => c.String());
            AddColumn("dbo.FootballLeague", "MoreInformation", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TriggerRule", "ToStartExecutorId", "dbo.TaskExecutor");
            DropForeignKey("dbo.TriggerRule", "FinishedExecutorId", "dbo.TaskExecutor");
            DropForeignKey("dbo.SchedulerTask", "TaskExecutorId", "dbo.TaskExecutor");
            DropIndex("dbo.TriggerRule", new[] { "ToStartExecutorId" });
            DropIndex("dbo.TriggerRule", new[] { "FinishedExecutorId" });
            DropIndex("dbo.SchedulerTask", new[] { "TaskExecutorId" });
            DropColumn("dbo.FootballLeague", "MoreInformation");
            DropColumn("dbo.FootballLeague", "Description");
            DropTable("dbo.TriggerRule");
            DropTable("dbo.TaskExecutor");
            DropTable("dbo.SchedulerTask");
        }
    }
}

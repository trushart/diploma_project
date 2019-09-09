namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLogtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        LogId = c.Long(nullable: false, identity: true),
                        Severity = c.Int(nullable: false),
                        Message = c.String(),
                        RequestAddress = c.String(),
                        IP = c.String(),
                        CallerMemberName = c.String(),
                        CallerProcessId = c.String(),
                        Data = c.String(),
                        Application = c.String(),
                    })
                .PrimaryKey(t => t.LogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Log");
        }
    }
}

namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPredictions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prediction",
                c => new
                    {
                        GameId = c.Int(nullable: false),
                        PredictionOption = c.Int(nullable: false),
                        PredictionOptionCoef = c.Double(nullable: false),
                        PredictionOptionCoefMax = c.Double(nullable: false),
                        PredictionOptionCoefMin = c.Double(nullable: false),
                        IsFinished = c.Boolean(nullable: false),
                        IsSucceed = c.Boolean(),
                    })
                .PrimaryKey(t => t.GameId)
                .ForeignKey("dbo.Game", t => t.GameId)
                .Index(t => t.GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prediction", "GameId", "dbo.Game");
            DropIndex("dbo.Prediction", new[] { "GameId" });
            DropTable("dbo.Prediction");
        }
    }
}

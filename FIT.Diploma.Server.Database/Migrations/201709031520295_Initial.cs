namespace FIT.Diploma.Server.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AverageRoundStats",
                c => new
                    {
                        LeagueSeasonId = c.Int(nullable: false),
                        HomeWinsCount_Average = c.Double(nullable: false),
                        DrawsCount_Average = c.Double(nullable: false),
                        AwayWinsCount_Average = c.Double(nullable: false),
                        HomeWinsCount_Max = c.Double(nullable: false),
                        DrawsCount_Max = c.Double(nullable: false),
                        AwayWinsCount_Max = c.Double(nullable: false),
                        HomeWinsCount_Min = c.Double(nullable: false),
                        DrawsCount_Min = c.Double(nullable: false),
                        AwayWinsCount_Min = c.Double(nullable: false),
                        Goals_Average = c.Double(nullable: false),
                        HomeGoals_Average = c.Double(nullable: false),
                        AwayGoals_Average = c.Double(nullable: false),
                        Goals_Max = c.Double(nullable: false),
                        HomeGoals_Max = c.Double(nullable: false),
                        AwayGoals_Max = c.Double(nullable: false),
                        Goals_Min = c.Double(nullable: false),
                        HomeGoals_Min = c.Double(nullable: false),
                        AwayGoals_Min = c.Double(nullable: false),
                        GamesOver2_5_Average = c.Double(nullable: false),
                        GamesUnder2_5_Average = c.Double(nullable: false),
                        GamesOver2_5_Max = c.Double(nullable: false),
                        GamesUnder2_5_Max = c.Double(nullable: false),
                        GamesOver2_5_Min = c.Double(nullable: false),
                        GamesUnder2_5_Min = c.Double(nullable: false),
                        BTTS_Yes_Average = c.Double(nullable: false),
                        BTTS_No_Average = c.Double(nullable: false),
                        BTTS_Yes_Max = c.Double(nullable: false),
                        BTTS_No_Max = c.Double(nullable: false),
                        BTTS_Yes_Min = c.Double(nullable: false),
                        BTTS_No_Min = c.Double(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LeagueSeasonId)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId)
                .Index(t => t.LeagueSeasonId);
            
            CreateTable(
                "dbo.LeagueSeason",
                c => new
                    {
                        LeagueSeasonId = c.Int(nullable: false, identity: true),
                        StartYear = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CountOfTeams = c.Int(nullable: false),
                        CountOfRounds = c.Int(nullable: false),
                        League_LeagueId = c.Int(),
                    })
                .PrimaryKey(t => t.LeagueSeasonId)
                .ForeignKey("dbo.FootballLeague", t => t.League_LeagueId)
                .Index(t => t.League_LeagueId);
            
            CreateTable(
                "dbo.FootballLeague",
                c => new
                    {
                        LeagueId = c.Int(nullable: false, identity: true),
                        LocationId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.LeagueId)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Country = c.String(),
                        City = c.String(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.BookmakerOdds",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        GameId = c.Int(nullable: false),
                        BookmakerId = c.Int(nullable: false),
                        HomeWinCoef = c.Double(nullable: false),
                        DrawCoef = c.Double(nullable: false),
                        AwayWinCoef = c.Double(nullable: false),
                        DoubleChanceCoef_1X = c.Double(nullable: false),
                        DoubleChanceCoef_12 = c.Double(nullable: false),
                        DoubleChanceCoef_X2 = c.Double(nullable: false),
                        BothTeamsToScore_Yes = c.Double(nullable: false),
                        BothTeamsToScore_No = c.Double(nullable: false),
                        Total2_5Over = c.Double(nullable: false),
                        Total2_5Under = c.Double(nullable: false),
                        HTHomeWinCoef = c.Double(nullable: false),
                        HTDrawCoef = c.Double(nullable: false),
                        HTAwayWinCoef = c.Double(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Bookmaker", t => t.BookmakerId, cascadeDelete: true)
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId)
                .Index(t => t.BookmakerId);
            
            CreateTable(
                "dbo.Bookmaker",
                c => new
                    {
                        BookmakerId = c.Int(nullable: false, identity: true),
                        BookmakerName = c.String(),
                    })
                .PrimaryKey(t => t.BookmakerId);

            CreateTable(
                "dbo.FootballTeam",
                c => new
                {
                    TeamId = c.Int(nullable: false, identity: true),
                    LocationId = c.Int(nullable: false),
                    Name = c.String(),
                    SourceId = c.String(),
                })
                .PrimaryKey(t => t.TeamId)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);

            CreateTable(
                "dbo.Game",
                c => new
                    {
                        GameId = c.Int(nullable: false, identity: true),
                        RoundId = c.Int(nullable: false),
                        HomeTeamId = c.Int(nullable: false),
                        AwayTeamId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Stadium = c.String(),
                        HomeTeamGoals = c.Int(nullable: false),
                        AwayTeamGoals = c.Int(nullable: false),
                        Result = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GameId)
                .ForeignKey("dbo.FootballTeam", t => t.AwayTeamId, cascadeDelete: false)
                .ForeignKey("dbo.FootballTeam", t => t.HomeTeamId, cascadeDelete: false)
                .ForeignKey("dbo.SeasonRound", t => t.RoundId, cascadeDelete: true)
                .Index(t => t.RoundId)
                .Index(t => t.HomeTeamId)
                .Index(t => t.AwayTeamId);   
            
            CreateTable(
                "dbo.SeasonRound",
                c => new
                    {
                        RoundId = c.Int(nullable: false, identity: true),
                        LeagueSeasonId = c.Int(nullable: false),
                        RoundNumber = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        GamePlayed = c.Int(nullable: false),
                        HomeWinsCount = c.Int(nullable: false),
                        DrawsCount = c.Int(nullable: false),
                        AwayWinsCount = c.Int(nullable: false),
                        Goals = c.Int(nullable: false),
                        HomeGoals = c.Int(nullable: false),
                        AwayGoals = c.Int(nullable: false),
                        GamesOver2_5 = c.Int(nullable: false),
                        GamesUnder2_5 = c.Int(nullable: false),
                        BTTS_Yes = c.Int(nullable: false),
                        BTTS_No = c.Int(nullable: false),
                        IsFinished = c.Boolean(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RoundId)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId, cascadeDelete: true)
                .Index(t => t.LeagueSeasonId);
            
            CreateTable(
                "dbo.BookmakerOddsStats",
                c => new
                    {
                        GameId = c.Int(nullable: false),
                        HomeWinCoef_Average = c.Double(nullable: false),
                        DrawCoef_Average = c.Double(nullable: false),
                        AwayWinCoef_Average = c.Double(nullable: false),
                        HomeWinCoef_Min = c.Double(nullable: false),
                        DrawCoef_Min = c.Double(nullable: false),
                        AwayWinCoef_Min = c.Double(nullable: false),
                        HomeWinCoef_Max = c.Double(nullable: false),
                        DrawCoef_Max = c.Double(nullable: false),
                        AwayWinCoef_Max = c.Double(nullable: false),
                        DoubleChanceCoef_1X_Average = c.Double(nullable: false),
                        DoubleChanceCoef_12_Average = c.Double(nullable: false),
                        DoubleChanceCoef_X2_Average = c.Double(nullable: false),
                        DoubleChanceCoef_1X_Min = c.Double(nullable: false),
                        DoubleChanceCoef_12_Min = c.Double(nullable: false),
                        DoubleChanceCoef_X2_Min = c.Double(nullable: false),
                        DoubleChanceCoef_1X_Max = c.Double(nullable: false),
                        DoubleChanceCoef_12_Max = c.Double(nullable: false),
                        DoubleChanceCoef_X2_Max = c.Double(nullable: false),
                        BothTeamsToScore_Yes_Average = c.Double(nullable: false),
                        BothTeamsToScore_No_Average = c.Double(nullable: false),
                        BothTeamsToScore_Yes_Min = c.Double(nullable: false),
                        BothTeamsToScore_No_Min = c.Double(nullable: false),
                        BothTeamsToScore_Yes_Max = c.Double(nullable: false),
                        BothTeamsToScore_No_Max = c.Double(nullable: false),
                        Total2_5Over_Average = c.Double(nullable: false),
                        Total2_5Under_Average = c.Double(nullable: false),
                        Total2_5Over_Min = c.Double(nullable: false),
                        Total2_5Under_Min = c.Double(nullable: false),
                        Total2_5Over_Max = c.Double(nullable: false),
                        Total2_5Under_Max = c.Double(nullable: false),
                        HTHomeWinCoef_Average = c.Double(nullable: false),
                        HTDrawCoef_Average = c.Double(nullable: false),
                        HTAwayWinCoef_Average = c.Double(nullable: false),
                        HTHomeWinCoef_Min = c.Double(nullable: false),
                        HTDrawCoef_Min = c.Double(nullable: false),
                        HTAwayWinCoef_Min = c.Double(nullable: false),
                        HTHomeWinCoef_Max = c.Double(nullable: false),
                        HTDrawCoef_Max = c.Double(nullable: false),
                        HTAwayWinCoef_Max = c.Double(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GameId)
                .ForeignKey("dbo.Game", t => t.GameId)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.FootballTeamForm",
                c => new
                    {
                        TeamId = c.Int(nullable: false),
                        LeagueSeasonId = c.Int(nullable: false),
                        TimePeriod = c.Int(nullable: false),
                        GamePlayed = c.Int(nullable: false),
                        WinsCount = c.Int(nullable: false),
                        DrawsCount = c.Int(nullable: false),
                        LossesCount = c.Int(nullable: false),
                        WinsPercentage = c.Double(nullable: false),
                        DrawsPercentage = c.Double(nullable: false),
                        LossesPercentage = c.Double(nullable: false),
                        Goals = c.Int(nullable: false),
                        GoalsPerGame = c.Double(nullable: false),
                        GoalsFor = c.Int(nullable: false),
                        GoalsForPerGame = c.Double(nullable: false),
                        GoalsAgainst = c.Int(nullable: false),
                        GoalsAgainstPerGame = c.Double(nullable: false),
                        GamesOver2_5 = c.Int(nullable: false),
                        GamesUnder2_5 = c.Int(nullable: false),
                        BTTS_Yes = c.Int(nullable: false),
                        BTTS_No = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeamId, t.LeagueSeasonId, t.TimePeriod })
                .ForeignKey("dbo.FootballTeam", t => t.TeamId, cascadeDelete: true)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.LeagueSeasonId);
            
            CreateTable(
                "dbo.GameRosourceId",
                c => new
                    {
                        GameId = c.Int(nullable: false),
                        Resource = c.String(nullable: false, maxLength: 128),
                        ResourceGameId = c.String(),
                    })
                .PrimaryKey(t => new { t.GameId, t.Resource })
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.GameStats",
                c => new
                    {
                        GameId = c.Int(nullable: false),
                        HalfTimeHomeGoals = c.Int(nullable: false),
                        HalfTimeAwayGoals = c.Int(nullable: false),
                        HalfTimeResult = c.Int(nullable: false),
                        HomeTeamShots = c.Int(nullable: false),
                        AwayTeamShots = c.Int(nullable: false),
                        HomeTeamTargetShots = c.Int(nullable: false),
                        AwayTeamTargetShots = c.Int(nullable: false),
                        HomeTeamFouls = c.Int(nullable: false),
                        AwayTeamFouls = c.Int(nullable: false),
                        HomeTeamCorners = c.Int(nullable: false),
                        AwayTeamCorners = c.Int(nullable: false),
                        HomeTeamYCards = c.Int(nullable: false),
                        AwayTeamYCards = c.Int(nullable: false),
                        HomeTeamRedCards = c.Int(nullable: false),
                        AwayTeamRedCards = c.Int(nullable: false),
                        HomeTeamOffsides = c.Int(nullable: false),
                        AwayTeamOffsides = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GameId)
                .ForeignKey("dbo.Game", t => t.GameId)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.HeadToHeadStats",
                c => new
                    {
                        Team1Id = c.Int(nullable: false),
                        Team2Id = c.Int(nullable: false),
                        GamePlayed = c.Int(nullable: false),
                        Team1WinsCount = c.Int(nullable: false),
                        DrawsCount = c.Int(nullable: false),
                        Team2WinsCount = c.Int(nullable: false),
                        Team1WinsPercentage = c.Double(nullable: false),
                        DrawsPercentage = c.Double(nullable: false),
                        Team2WinsPercentage = c.Double(nullable: false),
                        Goals = c.Int(nullable: false),
                        GoalsPerGame = c.Double(nullable: false),
                        Team1Goals = c.Int(nullable: false),
                        Team1GoalsPerGame = c.Double(nullable: false),
                        Team2Goals = c.Int(nullable: false),
                        Team2GoalsPerGame = c.Double(nullable: false),
                        GamesOver2_5 = c.Int(nullable: false),
                        GamesUnder2_5 = c.Int(nullable: false),
                        BTTS_Yes = c.Int(nullable: false),
                        BTTS_No = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team1Id, t.Team2Id })
                .ForeignKey("dbo.FootballTeam", t => t.Team1Id, cascadeDelete: false)
                .ForeignKey("dbo.FootballTeam", t => t.Team2Id, cascadeDelete: false)
                .Index(t => t.Team1Id)
                .Index(t => t.Team2Id);
            
            CreateTable(
                "dbo.LeagueSeasonReferees",
                c => new
                    {
                        RefereeId = c.Int(nullable: false),
                        LeagueSeasonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RefereeId, t.LeagueSeasonId })
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId, cascadeDelete: true)
                .ForeignKey("dbo.Referee", t => t.RefereeId, cascadeDelete: true)
                .Index(t => t.RefereeId)
                .Index(t => t.LeagueSeasonId);
            
            CreateTable(
                "dbo.Referee",
                c => new
                    {
                        RefereeId = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RefereeId);
            
            CreateTable(
                "dbo.LeagueSeasonTeams",
                c => new
                    {
                        CustomerId = c.Int(nullable: false),
                        LeagueSeasonId = c.Int(nullable: false),
                        TablePlace = c.Int(nullable: false),
                        GamePlayed = c.Int(nullable: false),
                        WinsCount = c.Int(nullable: false),
                        DrawsCount = c.Int(nullable: false),
                        LossesCount = c.Int(nullable: false),
                        GoalsFor = c.Int(nullable: false),
                        GoalsAgainst = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.CustomerId, t.LeagueSeasonId })
                .ForeignKey("dbo.FootballTeam", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.LeagueSeasonId);
            
            CreateTable(
                "dbo.ResourceConfiguration",
                c => new
                    {
                        ResourceConfigurationId = c.Int(nullable: false, identity: true),
                        ResourceDomain = c.String(),
                        ResourceDataFormat = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResourceConfigurationId);
            
            CreateTable(
                "dbo.ResourceProcessingStatus",
                c => new
                    {
                        ResourceProcessingStatusId = c.Int(nullable: false, identity: true),
                        ResourceConfigurationId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ProcessedMatches = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResourceProcessingStatusId)
                .ForeignKey("dbo.ResourceConfiguration", t => t.ResourceConfigurationId, cascadeDelete: true)
                .Index(t => t.ResourceConfigurationId);
            
            CreateTable(
                "dbo.SeasonStats",
                c => new
                    {
                        LeagueSeasonId = c.Int(nullable: false),
                        GamePlayed = c.Int(nullable: false),
                        HomeWinsCount = c.Int(nullable: false),
                        DrawsCount = c.Int(nullable: false),
                        AwayWinsCount = c.Int(nullable: false),
                        HomeWinsPercentage = c.Double(nullable: false),
                        DrawsPercentage = c.Double(nullable: false),
                        AwayWinsPercentage = c.Double(nullable: false),
                        Goals = c.Int(nullable: false),
                        HomeTeamsGoals = c.Int(nullable: false),
                        AwayTeamsGoals = c.Int(nullable: false),
                        GoalsPerGame = c.Double(nullable: false),
                        GamesOver2_5 = c.Int(nullable: false),
                        GamesUnder2_5 = c.Int(nullable: false),
                        BTTS_Yes = c.Int(nullable: false),
                        BTTS_No = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LeagueSeasonId)
                .ForeignKey("dbo.LeagueSeason", t => t.LeagueSeasonId)
                .Index(t => t.LeagueSeasonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SeasonStats", "LeagueSeasonId", "dbo.LeagueSeason");
            DropForeignKey("dbo.ResourceProcessingStatus", "ResourceConfigurationId", "dbo.ResourceConfiguration");
            DropForeignKey("dbo.LeagueSeasonTeams", "LeagueSeasonId", "dbo.LeagueSeason");
            DropForeignKey("dbo.LeagueSeasonTeams", "CustomerId", "dbo.FootballTeam");
            DropForeignKey("dbo.LeagueSeasonReferees", "RefereeId", "dbo.Referee");
            DropForeignKey("dbo.LeagueSeasonReferees", "LeagueSeasonId", "dbo.LeagueSeason");
            DropForeignKey("dbo.HeadToHeadStats", "Team2Id", "dbo.FootballTeam");
            DropForeignKey("dbo.HeadToHeadStats", "Team1Id", "dbo.FootballTeam");
            DropForeignKey("dbo.GameStats", "GameId", "dbo.Game");
            DropForeignKey("dbo.GameRosourceId", "GameId", "dbo.Game");
            DropForeignKey("dbo.FootballTeamForm", "LeagueSeasonId", "dbo.LeagueSeason");
            DropForeignKey("dbo.FootballTeamForm", "TeamId", "dbo.FootballTeam");
            DropForeignKey("dbo.BookmakerOddsStats", "GameId", "dbo.Game");
            DropForeignKey("dbo.BookmakerOdds", "GameId", "dbo.Game");
            DropForeignKey("dbo.Game", "RoundId", "dbo.SeasonRound");
            DropForeignKey("dbo.SeasonRound", "LeagueSeasonId", "dbo.LeagueSeason");
            DropForeignKey("dbo.Game", "HomeTeamId", "dbo.FootballTeam");
            DropForeignKey("dbo.Game", "AwayTeamId", "dbo.FootballTeam");
            DropForeignKey("dbo.FootballTeam", "LocationId", "dbo.Location");
            DropForeignKey("dbo.BookmakerOdds", "BookmakerId", "dbo.Bookmaker");
            DropForeignKey("dbo.AverageRoundStats", "LeagueSeasonId", "dbo.LeagueSeason");
            DropForeignKey("dbo.LeagueSeason", "League_LeagueId", "dbo.FootballLeague");
            DropForeignKey("dbo.FootballLeague", "LocationId", "dbo.Location");
            DropIndex("dbo.SeasonStats", new[] { "LeagueSeasonId" });
            DropIndex("dbo.ResourceProcessingStatus", new[] { "ResourceConfigurationId" });
            DropIndex("dbo.LeagueSeasonTeams", new[] { "LeagueSeasonId" });
            DropIndex("dbo.LeagueSeasonTeams", new[] { "CustomerId" });
            DropIndex("dbo.LeagueSeasonReferees", new[] { "LeagueSeasonId" });
            DropIndex("dbo.LeagueSeasonReferees", new[] { "RefereeId" });
            DropIndex("dbo.HeadToHeadStats", new[] { "Team2Id" });
            DropIndex("dbo.HeadToHeadStats", new[] { "Team1Id" });
            DropIndex("dbo.GameStats", new[] { "GameId" });
            DropIndex("dbo.GameRosourceId", new[] { "GameId" });
            DropIndex("dbo.FootballTeamForm", new[] { "LeagueSeasonId" });
            DropIndex("dbo.FootballTeamForm", new[] { "TeamId" });
            DropIndex("dbo.BookmakerOddsStats", new[] { "GameId" });
            DropIndex("dbo.SeasonRound", new[] { "LeagueSeasonId" });
            DropIndex("dbo.FootballTeam", new[] { "LocationId" });
            DropIndex("dbo.Game", new[] { "AwayTeamId" });
            DropIndex("dbo.Game", new[] { "HomeTeamId" });
            DropIndex("dbo.Game", new[] { "RoundId" });
            DropIndex("dbo.BookmakerOdds", new[] { "BookmakerId" });
            DropIndex("dbo.BookmakerOdds", new[] { "GameId" });
            DropIndex("dbo.FootballLeague", new[] { "LocationId" });
            DropIndex("dbo.LeagueSeason", new[] { "League_LeagueId" });
            DropIndex("dbo.AverageRoundStats", new[] { "LeagueSeasonId" });
            DropTable("dbo.SeasonStats");
            DropTable("dbo.ResourceProcessingStatus");
            DropTable("dbo.ResourceConfiguration");
            DropTable("dbo.LeagueSeasonTeams");
            DropTable("dbo.Referee");
            DropTable("dbo.LeagueSeasonReferees");
            DropTable("dbo.HeadToHeadStats");
            DropTable("dbo.GameStats");
            DropTable("dbo.GameRosourceId");
            DropTable("dbo.FootballTeamForm");
            DropTable("dbo.BookmakerOddsStats");
            DropTable("dbo.SeasonRound");
            DropTable("dbo.FootballTeam");
            DropTable("dbo.Game");
            DropTable("dbo.Bookmaker");
            DropTable("dbo.BookmakerOdds");
            DropTable("dbo.Location");
            DropTable("dbo.FootballLeague");
            DropTable("dbo.LeagueSeason");
            DropTable("dbo.AverageRoundStats");
        }
    }
}

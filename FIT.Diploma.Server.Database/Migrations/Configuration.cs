namespace FIT.Diploma.Server.Database.Migrations
{
    using System.Data.Entity.Migrations;
    using FIT.Diploma.Server.Database.SystemData;
    using FIT.Diploma.Server.Database.LeagueData;
    using FIT.Diploma.Server.Database.SchedulerData;
    using System;

    internal sealed class Configuration : DbMigrationsConfiguration<ServerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ServerDbContext context)
        {
            context.ResourceConfiguration.AddOrUpdate(
                  p => p.ResourceDomain,
                  new ResourceConfiguration { ResourceDomain = "http://www.football-data.co.uk", ResourceDataFormat = ResourceDataFormat.CSV }
                );
            context.Locations.AddOrUpdate(
                p => p.Country, 
                new Location { Country = "Spain" } 
                );
            
            context.FootballLeagues.AddOrUpdate(
                l => l.Name,
                new FootballLeague { LocationId = 1, Name = "Spain LaLiga" }
                );
            context.TaskExecutor.AddOrUpdate(
                te => te.Name,
                new TaskExecutor
                {
                    Name = "LeagueSeasonLinks",
                    CodeClass = "LeagueSeasonInfosGathering",
                    Description = "Task for gathering all links to files, that contains season stats (football-data resource)",
                    MaxRetries = 2,
                    TimeOut = 120
                },
                new TaskExecutor
                {
                    Name = "GameStatsGathering",
                    CodeClass = "GameStatsGathering",
                    Description = "Task for gathering games stats",
                    MaxRetries = 5,
                    TimeOut = 600
                },
                new TaskExecutor
                {
                    Name = "BwinCoefGathering",
                    CodeClass = "BwinCoefGathering",
                    Description = "Task for gathering games odds from Bwin.com",
                    MaxRetries = 5,
                    TimeOut = 600
                },
                new TaskExecutor
                {
                    Name = "SchedulerForBwin",
                    CodeClass = "SchedulerForBwin",
                    Description = "Helper task for scheduling next BwinCoefGathering runs",
                    MaxRetries = 2,
                    TimeOut = 30
                },
                new TaskExecutor
                {
                    Name = "SchedulerForGames",
                    CodeClass = "SchedulerForGames",
                    Description = "Helper task for scheduling next GameStatsGathering runs",
                    MaxRetries = 2,
                    TimeOut = 30
                },
                new TaskExecutor
                {
                    Name = "SchedulerForAnalyzers",
                    CodeClass = "SchedulerForAnalyzers",
                    Description = "Helper task for scheduling next RunAnalyzing runs",
                    MaxRetries = 2,
                    TimeOut = 120
                },
                new TaskExecutor
                {
                    Name = "BookmakerOddsStatsAnalyzing",
                    CodeClass = "BookmakerOddsStatsAnalyzing",
                    Description = "Task for run analyzing of bookmaker odds stats",
                    MaxRetries = 2,
                    TimeOut = 120
                }
                ,
                new TaskExecutor
                {
                    Name = "FootballTeamFormAnalyzing",
                    CodeClass = "FootballTeamFormAnalyzing",
                    Description = "Task for run analyzing of football teams stats",
                    MaxRetries = 2,
                    TimeOut = 120
                },
                new TaskExecutor
                {
                    Name = "GamePredictionAnalyzing",
                    CodeClass = "GamePredictionAnalyzing",
                    Description = "Task for run game prediction analysis",
                    MaxRetries = 2,
                    TimeOut = 120
                },
                new TaskExecutor
                {
                    Name = "HeadToHeadStatsAnalyzing",
                    CodeClass = "HeadToHeadStatsAnalyzing",
                    Description = "Task for run analyzing of football teams stats",
                    MaxRetries = 2,
                    TimeOut = 120
                },
                new TaskExecutor
                {
                    Name = "RoundStatsAnalyzing",
                    CodeClass = "RoundStatsAnalyzing",
                    Description = "Task for run analyzing of rounds stats",
                    MaxRetries = 2,
                    TimeOut = 120
                },
                new TaskExecutor
                {
                    Name = "SeasonStatsAnalyzing",
                    CodeClass = "SeasonStatsAnalyzing",
                    Description = "Task for run analyzing of season stats",
                    MaxRetries = 2,
                    TimeOut = 120
                }
                );
            
            context.TriggerRule.AddOrUpdate(
                tr => new { tr.FinishedExecutorId, tr.ToStartExecutorId },
                new TriggerRule
                {
                    FinishedExecutorId = 1,
                    ToStartExecutorId = 1,
                    DelayTime = 60 * 60 * 24 * 7,
                    CheckConfigs = false,
                    WhenFinishedExecutorUpdateDb = null
                },
                new TriggerRule
                {
                    FinishedExecutorId = 2,
                    ToStartExecutorId = 5,
                    DelayTime = 120,
                    CheckConfigs = true,
                    WhenFinishedExecutorUpdateDb = null
                },
                new TriggerRule
                {
                    FinishedExecutorId = 3,
                    ToStartExecutorId = 4,
                    DelayTime = 120,
                    CheckConfigs = true,
                    WhenFinishedExecutorUpdateDb = null
                },
                new TriggerRule
                {
                    FinishedExecutorId = 3,
                    ToStartExecutorId = 5,
                    DelayTime = 120,
                    CheckConfigs = true,
                    WhenFinishedExecutorUpdateDb = true
                },
                new TriggerRule
                {
                    FinishedExecutorId = 2,
                    ToStartExecutorId = 6,
                    DelayTime = 20,
                    CheckConfigs = true,
                    WhenFinishedExecutorUpdateDb = null //true
                }
                );

            context.SchedulerTask.AddOrUpdate(
                st => new { st.TaskExecutorId, st.Status },
                new SchedulerTask
                {
                    Status = TaskStatus.Wait,
                    TaskExecutorId = 1,
                    PlanningTime = DateTime.Now,
                    UpdateDb = null,
                    Result = null,
                    Type = TaskType.Crawler
                },
                new SchedulerTask
                {
                    Status = TaskStatus.Wait,
                    TaskExecutorId = 2,
                    PlanningTime = DateTime.Now.AddSeconds(20),
                    UpdateDb = null,
                    Result = null,
                    Type = TaskType.Crawler
                },
                new SchedulerTask
                {
                    Status = TaskStatus.Wait,
                    TaskExecutorId = 3,
                    PlanningTime = DateTime.Now.AddSeconds(30),
                    UpdateDb = null,
                    Result = null,
                    Type = TaskType.Crawler
                }
                );
            /**/
        }
    }
}

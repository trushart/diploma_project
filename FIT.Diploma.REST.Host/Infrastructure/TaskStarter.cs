using FIT.Diploma.REST.Host.Helpers;
using FIT.Diploma.REST.Host.TaskExecutors;
using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.SchedulerData;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskStatus = FIT.Diploma.Server.Database.SchedulerData.TaskStatus;

namespace FIT.Diploma.REST.Host.Infrastructure
{
    public class TaskStarter
    {
        private static bool runLoop;
        private SchedulerDataRepository repo;
        private const int TASK_WAITING_DELAY = 15000; //ms
        private const int NO_TASK_DELAY = 1800000; //ms
        private const int EXCEPTION_DELAY = 5000; //ms
        private Dictionary<string, ITaskExecutor> taskExecutors;
        ConcurrentStack<Task> tasks;

        private void Initialize()
        {
            Console.WriteLine("***** Initialize all TaskStarter variables *****");
            repo = new SchedulerDataRepository();
            runLoop = true;
            tasks = new ConcurrentStack<Task>();
            taskExecutors = new Dictionary<string, ITaskExecutor>();
            taskExecutors.Add(nameof(GameStatsGathering), new GameStatsGathering());
            taskExecutors.Add(nameof(BwinCoefGathering), new BwinCoefGathering());
            taskExecutors.Add(nameof(LeagueSeasonInfosGathering), new LeagueSeasonInfosGathering());
            taskExecutors.Add(nameof(SchedulerForGames), new SchedulerForGames());
            taskExecutors.Add(nameof(SchedulerForBwin), new SchedulerForBwin());
            taskExecutors.Add(nameof(SchedulerForAnalyzers), new SchedulerForAnalyzers());
            taskExecutors.Add(nameof(BookmakerOddsStatsAnalyzing), new BookmakerOddsStatsAnalyzing());
            taskExecutors.Add(nameof(FootballTeamFormAnalyzing), new FootballTeamFormAnalyzing());
            taskExecutors.Add(nameof(GamePredictionAnalyzing), new GamePredictionAnalyzing());
            taskExecutors.Add(nameof(HeadToHeadStatsAnalyzing), new HeadToHeadStatsAnalyzing());
            taskExecutors.Add(nameof(RoundStatsAnalyzing), new RoundStatsAnalyzing());
            taskExecutors.Add(nameof(SeasonStatsAnalyzing), new SeasonStatsAnalyzing());
        }

        public void Stop()
        {
            runLoop = false;
        }

        public async Task LoopWaiting()
        {
            while (runLoop)
            {
                if (!tasks.Any())
                    await Task.Delay(5000);
                else
                {
                    Task task;
                    tasks.TryPop(out task);
                    await task;
                }
            }
        }

        public async Task Start()
        {
            Initialize();
            TimeSpan delay;
            bool taskInProgress = false;
            int noTaskLoops = 0;
            Task waiting = LoopWaiting();

            while (runLoop)
            {
                try
                {
                    var nextTask = repo.GetNextTask();
                    taskInProgress = repo.AnyTaskInProgress();

                    if (nextTask == null)
                    {
                        var waitingTime = taskInProgress ? TASK_WAITING_DELAY : NO_TASK_DELAY;
                        if (!taskInProgress)
                        {
                            if (++noTaskLoops == 6) break;
                        }
                        await Wait(waitingTime);
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"Next task is '{nextTask.TaskExecutor.Name}'");
                        delay = GetDelayTime(nextTask.PlanningTime);
                        if (taskInProgress && delay.TotalMilliseconds > TASK_WAITING_DELAY){
                            //some task is in progress, so may be it finished soon and will trigger new tasks
                            await Wait(TASK_WAITING_DELAY);
                            continue;
                        }
                        if (delay.TotalMilliseconds > NO_TASK_DELAY)
                        {
                            await Wait(NO_TASK_DELAY);
                            continue;
                        }
                        if (delay.Seconds > 0) await Wait(delay);
                        ExecuteScheduledTask(nextTask);
                    }
                    noTaskLoops = 0;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    await Task.Delay(EXCEPTION_DELAY);
                }                
            }
        }

        private TimeSpan GetDelayTime(DateTime taskStartTime)
        {
            var currentTime = DateTime.Now;
            return new TimeSpan(taskStartTime.Ticks - currentTime.Ticks);
        }

        private async Task Wait(int delay)
        {
            Console.WriteLine($"Going to wait {delay} ms. Current time: {DateTime.Now.ToString("o")}");
            await Task.Delay(delay);
            Console.WriteLine($"Woke up. Current time: {DateTime.Now.ToString("o")}");
        }

        private async Task Wait(TimeSpan delay)
        {
            Console.WriteLine($"Going to wait {delay} ms. Current time: {DateTime.Now.ToString("o")}");
            await Task.Delay(delay);
            Console.WriteLine($"Woke up. Current time: {DateTime.Now.ToString("o")}");
        }        

        private void ExecuteScheduledTask(SchedulerTask taskDb)
        {
            int attempts = 0;
            var executor = repo.GetTaskExecutor(taskDb.TaskExecutorId);
            Console.WriteLine($"***** Execute scheduled task [{taskDb.TaskId}] *****");
            taskDb = repo.StartTask(taskDb);
            while (true)
            {
                Console.WriteLine($"***** Run TaskExecutor '{executor.Name}' *****");
                if (!taskExecutors.ContainsKey(executor.CodeClass))
                {
                    Console.WriteLine($"Couldn't run TaskExecutor '{executor.Name}', codeClass of executor '{executor.CodeClass}' is unknown.");
                    throw new Exception("Unknown Task Executor");
                }

                var taskExecutor = taskExecutors[executor.CodeClass];
                var task = Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        taskDb.StartTime = DateTime.Now;
                        try
                        {
                            var result = await taskExecutor.Execute().TimeoutAfter(executor.TimeOut, taskExecutor);
                            if (result.IsFailed)
                                throw new Exception(result.FailedReason);
                            taskDb.Status = TaskStatus.Finished;
                            taskDb.StartTime = result.StartTime;
                            taskDb.EndTime = result.EndTime;
                            taskDb.UpdateDb = result.UpdateDb;
                            taskDb.FailReason = result.FailedReason;
                            taskDb.Result = result.IsFailed
                                ? Server.Database.SchedulerData.TaskResult.Failed
                                : Server.Database.SchedulerData.TaskResult.Succeed;
                            repo.UpdateTask(taskDb);
                            CreateTriggeredTasks(executor.TaskExecutorId, result.UpdateDb);
                            return;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"TaskExecutor run fails. Retry attempts {attempts++} from {executor.MaxRetries}. Exception: {ex.Message}");
                            if (attempts > executor.MaxRetries)
                            {
                                taskDb.Status = TaskStatus.Finished;
                                taskDb.EndTime = DateTime.Now;
                                taskDb.UpdateDb = false;
                                taskDb.FailReason = ex.Message;
                                taskDb.Result = Server.Database.SchedulerData.TaskResult.Failed;
                                repo.UpdateTask(taskDb);
                                CreateTriggeredTasks(executor.TaskExecutorId, false);
                                return;
                            }
                            Task.Delay(EXCEPTION_DELAY).Wait();
                        }
                    }                        
                }, TaskCreationOptions.AttachedToParent).Unwrap();
                        
                tasks.Push(task);
                break;
            }                
        }

        private void CreateTriggeredTasks(int finishedExecutorId, bool updatedDb)
        {
            Console.WriteLine($"***** Trigger next tasks after finish of TaskExecutor [{finishedExecutorId}] *****");
            var triggerRules = repo.GetTriggersByExecutorId(finishedExecutorId);

            foreach(var rule in triggerRules)
            {
                if (rule.WhenFinishedExecutorUpdateDb.HasValue)
                {
                    if (rule.WhenFinishedExecutorUpdateDb.Value != updatedDb)
                        continue;
                }

                var plannedTasks = repo.GetPlannedTasks();
                if (plannedTasks.Any(task => task.TaskExecutorId == rule.ToStartExecutorId))
                    continue;

                SchedulerTask newTask = new SchedulerTask
                {
                    TaskExecutorId = rule.ToStartExecutorId,
                    Status = TaskStatus.Wait,
                    PlanningTime = DateTime.Now.AddSeconds(rule.DelayTime),
                    UpdateDb = null,
                    Result = null,
                    Type = TaskType.Crawler
                };
                repo.AddSchedulerTask(newTask);
            }
        }       
    }
}

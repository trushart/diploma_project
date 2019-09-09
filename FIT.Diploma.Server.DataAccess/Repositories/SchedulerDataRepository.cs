using FIT.Diploma.Server.Database.SchedulerData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIT.Diploma.Server.DataAccess.Repositories
{
    public class SchedulerDataRepository : BaseRepository
    {
        private void OpenConnection() { if (Db.Database.Connection.State == System.Data.ConnectionState.Closed) Db.Database.Connection.Open(); }

        public List<TriggerRule> GetTriggersByExecutorId(int finishedExecutorId)
        {
            OpenConnection();

            var result = new List<TriggerRule>();
            try
            {
                result = (from n in Db.TriggerRule
                          where n.FinishedExecutorId == finishedExecutorId
                          select n).ToList();
            }
            catch (Exception ex)
            {
                var repo = new LogRepository();
                repo.WriteExceptionLog(ex);
                throw;
            }

            return result;
        }

        public TaskExecutor GetTaskExecutor(int executorId)
        {
            OpenConnection();

            var result = (from n in Db.TaskExecutor
                          where n.TaskExecutorId == executorId
                          select n).FirstOrDefault();

            if (result == null)
            {
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Warning, $"Could not found TaskExecutro with id={executorId}.",
                    "GetTaskExecutor", "", "", "");
            }

            return result;
        }

        public TaskExecutor GetTaskExecutor(string executorName)
        {
            OpenConnection();

            var result = (from n in Db.TaskExecutor
                          where n.Name == executorName
                          select n).FirstOrDefault();

            if (result == null)
            {
                var repo = new LogRepository();
                repo.WriteLog(Database.SystemData.Severity.Warning, $"Could not found TaskExecutro with name={executorName}.",
                    "GetTaskExecutor", "", "", "");
            }

            return result;
        }

        private static object syncLockSchedulerTask = new object();

        public bool AddSchedulerTask(SchedulerTask task)
        {
            bool result = false;
            lock (syncLockSchedulerTask)
            {
                OpenConnection();

                if (!IsSchedulerTaskExist(task))
                {
                    Db.SchedulerTask.Add(task);
                    Db.SaveChanges();
                    result = true;
                }                

                Db.Database.Connection.Close();
            }
            return result;
        }

        public DateTime? GetTimeOfLastExecution(string executorName)
        {
            DateTime? result = null;

            OpenConnection();

            var lastExecution = (from t in Db.SchedulerTask
                                 join te in Db.TaskExecutor
                                    on t.TaskExecutorId equals te.TaskExecutorId
                                 where te.Name == executorName && t.Status == TaskStatus.Finished
                                 orderby t.EndTime descending
                                 select t).FirstOrDefault();

            if (lastExecution != null)
                result = lastExecution.EndTime;

            Db.Database.Connection.Close();
            return result;
        }

        private bool IsSchedulerTaskExist(SchedulerTask task)
        {
            var downBoundary = task.PlanningTime.AddMinutes(-40);
            var upBoundary = task.PlanningTime.AddMinutes(40);
            return (from t in Db.SchedulerTask
                    where
                        downBoundary < t.PlanningTime
                        && upBoundary > t.PlanningTime
                        && t.TaskExecutorId == task.TaskExecutorId
                        && t.Status == task.Status
                    select t).Any();
        }

        public List<SchedulerTask> GetPlannedTasks()
        {
            lock (syncLockSchedulerTask)
            {
                OpenConnection();

                var result = new List<SchedulerTask>();
                try
                {
                    result = (from n in Db.SchedulerTask
                              where n.Status == Database.SchedulerData.TaskStatus.Wait
                              select n).ToList();

                    foreach(var task in result)
                    {
                        if (task.TaskExecutor == null)
                            task.TaskExecutor = GetTaskExecutor(task.TaskExecutorId);
                    }
                }
                catch (Exception ex)
                {
                    var repo = new LogRepository();
                    repo.WriteExceptionLog(ex);
                    throw;
                }

                Db.Database.Connection.Close();
                return result;
            }            
        }

        public SchedulerTask StartTask(SchedulerTask task)
        {
            lock (syncLockSchedulerTask)
            {
                OpenConnection();

                var result = (from n in Db.SchedulerTask
                              where n.TaskId == task.TaskId
                              select n).FirstOrDefault();

                if (result != null)
                {
                    result.StartTime = DateTime.Now;
                    result.Status = Database.SchedulerData.TaskStatus.InProgress;
                    Db.SaveChanges();
                }

                Db.Database.Connection.Close();
                return result;
            }            
        }

        public void UpdateTask(SchedulerTask task)
        {
            lock (syncLockSchedulerTask)
            {
                OpenConnection();

                var result = (from n in Db.SchedulerTask
                              where n.TaskId == task.TaskId
                              select n).FirstOrDefault();

                if (result != null)
                {
                    result.CopuFrom(task);
                    Db.SaveChanges();
                }
                Db.Database.Connection.Close();
            }            
        }

        public SchedulerTask GetNextTask()
        {
            return GetPlannedTasks().OrderBy(n => n.PlanningTime).FirstOrDefault();
        }

        public bool AnyTaskInProgress()
        {
            lock (syncLockSchedulerTask)
            {
                OpenConnection();

                bool result = false;
                try
                {
                    result = (from n in Db.SchedulerTask
                              where n.Status == Database.SchedulerData.TaskStatus.InProgress
                              select n).Any();
                }
                catch (Exception ex)
                {
                    var repo = new LogRepository();
                    repo.WriteExceptionLog(ex);
                    throw;
                }

                Db.Database.Connection.Close();
                return result;
            }            
        }
    }
}

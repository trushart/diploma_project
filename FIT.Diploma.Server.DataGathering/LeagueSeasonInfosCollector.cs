using FIT.Diploma.Server.DataGathering.Helpers;
using System;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataGathering
{
    public class LeagueSeasonInfosCollector : IDataCollector
    {
        //Timeout = 10 minutes
        public int GetTimeout() => 10 * 60;
        private Task<bool> task;

        public bool Start()
        {
            bool result = true;

            try
            {
                task = CollectGameStats();
                result = task.Result;
                Console.WriteLine("Collecting of data succeed.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Collecting of data canceled.");
            }
            catch (Exception ex)
            {
                task = null;
                Console.WriteLine("Collecting of data failed.\r\nException: " + ex.Message);
            }

            return result;
        }

        public void Stop()
        {
            Console.WriteLine("Canceling data gathering.");
        }

        private Task<bool> CollectGameStats()
        {
            return Task.Run(() =>
            {
                bool result = false;
                Console.WriteLine("Start FootballDataManager.");
                FootballDataManager manager = new FootballDataManager();
                try
                {
                    manager.SaveAllFileLinks();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CollectGameStats] Exception: {ex.Message}");
                }
                Console.WriteLine("Stop FootballDataManager.");
                return result;
            });           
        }
    }
}

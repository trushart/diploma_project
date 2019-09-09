using System;
using System.Threading;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataGathering
{
    public class SeasonGamesCollector : IDataCollector
    {
        //Timeout = 10 minutes
        public int GetTimeout() => 10 * 60;
        private Task<bool> task;

        public bool Start()
        {
            var result = false;

            try
            {
                task = CollectSeasonGamesShedule();
                result = task.Result;
                Console.WriteLine("Collecting of data succeed.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Collecting of data canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Collecting of data failed.\r\nException: " + ex.Message);
            }

            return result;
        }

        public void Stop()
        {
            task = null;
            Console.WriteLine("Canceling data gathering.");
        }

        private Task<bool> CollectSeasonGamesShedule()
        {
            return Task.Run(() => {
                var result = false;
                Console.WriteLine("Start BwinParser.");
                Console.WriteLine("Stop BwinParser.");
                return result;
            });            
        }
    }
}

using FIT.Diploma.Server.DataGathering.Helpers;
using System;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataGathering
{
    public class BookmakerOddsCollector : IDataCollector
    {
        //Timeout = 10 minutes
        public int GetTimeout() => 10 * 60;
        private Task<bool> task;

        public bool Start()
        {
            bool result = false;
            try
            {
                task = CollectBookmakerOddsData();
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

        private Task<bool> CollectBookmakerOddsData()
        {
            return Task.Run(() =>
            {
                var result = false;
                BwinOddsParser parser = new BwinOddsParser();
                Console.WriteLine("Start BwinParser.");
                var allLinks = parser.GetAndSaveAllAvailableGameLinks();
                foreach (var link in allLinks)
                {
                    Console.WriteLine("Found link: " + link);
                    result |= parser.ParseAndSaveGameOdds(link);
                }
                Console.WriteLine("Stop BwinParser.");
                return result;
            });            
        }
    }
}

using FIT.Diploma.REST.Host.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FIT.Diploma.REST.Host
{
    public class AllServices : ServiceControl
    {
        //initialize all services
        private readonly RestApp restApp;
        private TaskStarter taskStarter;
        private Task m_gatheringTask;

        public AllServices()
        {
            restApp = new RestApp();
            taskStarter = new TaskStarter();
        }

        public bool Start(HostControl hostControl)
        {
            var success = true;

            try
            {
                restApp.Start();                
                m_gatheringTask = taskStarter.Start();
                Console.WriteLine("Start Gathering service.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Could not start Rest Service. Ecxception: {ex.Message}");
                success = false;
            }

            return success;
        }

        public bool Stop(HostControl hostControl)
        {
            var success = true;

            try
            {
                restApp.Stop();
                taskStarter.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not start Rest Service. Ecxception: {ex.Message}");
                success = false;
            }

            return success;
        }
    }
}

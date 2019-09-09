using FIT.Diploma.REST.Host.Infrastructure;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FIT.Diploma.REST.Host
{
    public class RestApp
    {
        private NancyHost m_nancyHost;       

        public void Start()
        {
            var configs = new HostConfiguration();
            configs.UrlReservations.CreateAutomatically = true;
            m_nancyHost = new NancyHost(configs, new Uri(@"http://localhost:8888"));
            m_nancyHost.Start();
            Console.WriteLine("Start REST service.");          
        }

        public void Stop()
        {
            m_nancyHost.Stop();
            Console.WriteLine("Stop REST service.");            
        }
    }
}

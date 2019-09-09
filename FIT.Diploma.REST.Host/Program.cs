using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using FIT.Diploma.REST.Host.Helpers;

namespace FIT.Diploma.REST.Host
{
    internal class Program
    {
        public const int DefaultTimeoutSeconds = 120;

        public static void Main()
        {
            AutoMapperConfig.Initialize();
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            var exitCode = HostFactory.Run(host =>
            {
                // Provide the service's behavior using our custom
                //  ServiceHost class
                //
                host.Service<AllServices>(service =>
                {
                    service.ConstructUsing(name => new AllServices());
                    service.WhenStarted((app, hostControl) => app.Start(hostControl));
                    service.WhenStopped((app, hostControl) => app.Stop(hostControl));
                });

                // Now define some attributes of the service overall
                //
                host.RunAsLocalSystem();

                // Provide the metadata to the service control
                //
                host.SetServiceName("Football-data Rest service");
                host.SetDescription("A custom service that provides football data");

            });

            Console.ReadKey();
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exceptionObject = (Exception)e.ExceptionObject;

            if (e.IsTerminating)
            {
                Console.WriteLine($"[Fatal] Host exception: {exceptionObject.Message}\r\nStackTrace: {exceptionObject.StackTrace}");
            }
            else
            {
                Console.WriteLine($"[Error] Host exception: {exceptionObject.Message}\r\nStackTrace: {exceptionObject.StackTrace}");
            }
        }
    }
}

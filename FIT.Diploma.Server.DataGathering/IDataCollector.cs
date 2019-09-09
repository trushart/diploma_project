using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataGathering
{
    public interface IDataCollector
    {
        /// <summary>
        /// Method for starting data gathering process.
        /// </summary>
        bool Start();
        /// <summary>
        /// Method for stopping data gathering process.
        /// </summary>
        void Stop();
        /// <summary>
        /// Timeout - limit of time for data gathering porcess run. 
        /// If data collector couldn't finish gathering - it should be stopped outside. 
        /// </summary>
        /// <returns>timeout in seconds</returns>
        int GetTimeout();
    }
}

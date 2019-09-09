using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataGathering.Helpers
{
    public class DataSourceFileLoader
    {
        private readonly string DataSourceUrl;

        public DataSourceFileLoader(string url)
        {
            DataSourceUrl = url;
        }

        public bool DownloadFile(string fileUrl, string fileName)
        {
            WebClient client = new WebClient();
            string myStringWebResource = DataSourceUrl + fileUrl;
            Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......", fileUrl, myStringWebResource);
            
            // Download the Web resource and save it into the current filesystem folder.
            try
            {
                client.DownloadFile(myStringWebResource, fileName);
            }
            catch(Exception ex)
            {
                Console.WriteLine("WebClient.DownloadFile failed. ");
                return false;
            }
            
            return true;
        }
    }
}

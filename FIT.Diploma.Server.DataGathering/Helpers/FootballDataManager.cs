using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.SystemData;
using System;

namespace FIT.Diploma.Server.DataGathering.Helpers
{
    public class FootballDataManager
    {
        //return true if Db was updated
        public bool SaveAllFileLinks()
        {
            bool dbUpdated = false;
            int counter = 0;
            SystemDataRepository repo = new SystemDataRepository();
            
            FootballDataParser parser = new FootballDataParser();
            var config = repo.GetResourceConfig(parser.GetDomain);
            Console.WriteLine("Start FootballDataParser.");
            foreach (var link in parser.GetAllLinks("1.csv"))
            {
                Console.WriteLine("link [{0}]: {1}", counter++, link);
                dbUpdated |= repo.AddResourceProcessingStatus(link, config);
            }
            Console.WriteLine("Stop FootballDataParser.");
            return dbUpdated;
        }

        public bool ProcessLink(ref ResourceProcessingStatus resource)
        {
            string linkUrl = resource.ResourceConfiguration.ResourceDomain + "/" + resource.TargetUrl;
            Console.WriteLine("Process link: " + linkUrl);

            //link has format ".../1011/SP1.csv", where 1011 is season 2010-2011
            string season = resource.TargetUrl.Substring(resource.TargetUrl.Length - 12, 4); 

            //download file
            DataSourceFileLoader loader = new DataSourceFileLoader(resource.ResourceConfiguration.ResourceDomain);
            if (loader.DownloadFile("/" + resource.TargetUrl, season + ".csv"))
                Console.WriteLine("Downloading file {0} succeed", linkUrl);
            else
            {
                Console.WriteLine("Downloading file {0} failes", linkUrl);
                return false;
            }

            FootballDataCSVParser parser = new FootballDataCSVParser();
            int processedGames;
            parser.ParseCsvAndSaveToDB(season + ".csv", season, out processedGames);

            resource.ProcessedMatches += processedGames;
            resource.Status = resource.ProcessedMatches == 380 || resource.ProcessedMatches == 462 ? ProcessingStatus.Finished : ProcessingStatus.Start;
            return processedGames > 0;
        }

        private SystemDataRepository repo = new SystemDataRepository();

        public bool ProcessAllLinks()
        {
            var result = false;
            var links = repo.GetAllResources();
            Console.WriteLine("Count of links: " + links.Count);
            for(int i = 0; i < links.Count; i++)
            {
                var link = links[i];
                if(link.Status == ProcessingStatus.Finished)
                {
                    Console.WriteLine($"[ProcessAllLinks] link ({link.TargetUrl}) has Finished status. Skipped.");
                    continue;
                }
                link.Status = ProcessingStatus.Processing;
                result |= ProcessLink(ref link);
                repo.UpdateResource(link);
            }
            return result;
        }
    }
}

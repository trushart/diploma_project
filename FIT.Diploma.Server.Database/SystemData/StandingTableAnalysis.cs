using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.Database.SystemData
{
    public class StandingTableAnalysis
    {
        [Key]
        [ForeignKey("LeagueSeason")]
        public int LeagueSeasonId { get; set; }

        public bool AnalysisDone { get; set; }

        public DateTime LastUpdate { get; set; }

        public DateTime Created { get; set; }

        public string Data { get; set; }

        public virtual LeagueSeason LeagueSeason { get; set; }

        public void Copy(StandingTableAnalysis standingTable)
        {
            if (LeagueSeasonId == standingTable.LeagueSeasonId)
            {
                AnalysisDone = standingTable.AnalysisDone;
                LastUpdate = DateTime.Now;
                Data = standingTable.Data;
            }
        }
    }
}

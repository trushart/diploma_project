using FIT.Diploma.Server.Database.LeagueData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.SystemData
{
    public class TeamForm_AnalyzedSeasons
    {
        [Key]
        [ForeignKey("LeagueSeason")]
        public int LeagueSeasonId { get; set; }

        public bool AnalysisFinished { get; set; }

        public virtual LeagueSeason LeagueSeason { get; set; }
    }
}

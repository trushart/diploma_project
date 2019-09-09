using FIT.Diploma.Server.Database.LeagueData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.SystemData
{
    public class SeasonRound_AnalyzedGames
    {
        [Key]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        public int LeagueSeasonId { get; set; }

        public bool AnalysisFinished { get; set; }

        public virtual Game Game { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.LeagueData
{
    public class LeagueSeasonReferees
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Referee")]
        public int RefereeId { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("LeagueSeason")]
        public int LeagueSeasonId { get; set; }

        public virtual Referee Referee { get; set; }

        public virtual LeagueSeason LeagueSeason { get; set; }
    }
}

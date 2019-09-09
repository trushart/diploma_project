using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.LeagueData
{
    public class FootballLeague
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeagueId { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string MoreInformation { get; set; }

        public virtual Location Location { get; set; }
    }
}

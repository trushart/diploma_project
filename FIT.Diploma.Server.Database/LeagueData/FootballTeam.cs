using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.LeagueData
{
    public class FootballTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public string Name { get; set; }

        public string SourceId { get; set; }

        public string Data { get; set; }

        public int? ParentTeamId { get; set; }

        public virtual Location Location { get; set; }
    }
}

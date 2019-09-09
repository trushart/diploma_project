using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.LeagueData
{
    public class GameRosourceId
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string Resource { get; set; }

        public string ResourceGameId { get; set; }

        public virtual Game Game { get; set; }
    }
}

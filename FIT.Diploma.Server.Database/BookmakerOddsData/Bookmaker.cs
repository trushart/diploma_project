using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.BookmakerOddsData
{
    public class Bookmaker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookmakerId { get; set; }

        public string BookmakerName { get; set; }
    }
}

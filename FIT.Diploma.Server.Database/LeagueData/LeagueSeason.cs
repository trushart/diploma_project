using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.LeagueData
{
    public class LeagueSeason
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeagueSeasonId { get; set; }

        public int StartYear { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CountOfTeams { get; set; }

        public int CountOfRounds { get; set; }

        public virtual FootballLeague League { get; set; }

        public void Copy(LeagueSeason season)
        {
            if(LeagueSeasonId == season.LeagueSeasonId)
            {
                StartYear = season.StartYear;
                StartDate = season.StartDate;
                EndDate = season.EndDate;
                CountOfRounds = season.CountOfRounds;
                CountOfTeams = season.CountOfTeams;
            }
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.LeagueData
{
    public class LeagueSeasonTeams
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("FootballTeam")]
        public int FootballTeamId { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("LeagueSeason")]
        public int LeagueSeasonId { get; set; }

        public int TablePlace { get; set; }

        public int GamePlayed { get; set; }

        public int WinsCount { get; set; }

        public int DrawsCount { get; set; }

        public int LossesCount { get; set; }

        public int GoalsFor { get; set; }

        public int GoalsAgainst { get; set; }

        public int Points { get; set; }

        public DateTime LastUpdate { get; set; }

        public virtual FootballTeam FootballTeam { get; set; }

        public virtual LeagueSeason LeagueSeason { get; set; }

        public void Copy(LeagueSeasonTeams team)
        {
            if(FootballTeamId == team.FootballTeamId && LeagueSeasonId == team.LeagueSeasonId)
            {
                TablePlace = team.TablePlace;
                GamePlayed = team.GamePlayed;
                WinsCount = team.WinsCount;
                DrawsCount = team.DrawsCount;
                LossesCount = team.LossesCount;
                GoalsFor = team.GoalsFor;
                GoalsAgainst = team.GoalsAgainst;
                Points = team.Points;
                LastUpdate = DateTime.Now;
            }
        }
    }
}

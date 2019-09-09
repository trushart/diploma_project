using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.LeagueData
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; }

        [ForeignKey("SeasonRound")]
        public int RoundId { get; set; }

        [ForeignKey("HomeFootballTeam")]
        public int HomeTeamId { get; set; }

        [ForeignKey("AwayFootballTeam")]
        public int AwayTeamId { get; set; }

        public DateTime Date { get; set; }

        public string Stadium { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public GameResult Result { get; set; }

        public virtual SeasonRound SeasonRound { get; set; }

        public virtual FootballTeam HomeFootballTeam { get; set; }

        public virtual FootballTeam AwayFootballTeam { get; set; }

        public void Copy(Game game)
        {
            HomeTeamGoals = game.HomeTeamGoals;
            AwayTeamGoals = game.AwayTeamGoals;
            Result = game.Result;
        }
    }

    public enum GameResult
    {
        HomeWin = 1,
        Draw = 2,
        AwayWin =3
    }
}

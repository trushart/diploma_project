using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.LeagueData
{
    public class GameStats
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        public int HalfTimeHomeGoals { get; set; }
        public int HalfTimeAwayGoals { get; set; }
        public GameResult HalfTimeResult { get; set; }

        public int HomeTeamShots { get; set; }
        public int AwayTeamShots { get; set; }
        public int HomeTeamTargetShots { get; set; }
        public int AwayTeamTargetShots { get; set; }

        public int HomeTeamFouls { get; set; }
        public int AwayTeamFouls { get; set; }

        public int HomeTeamCorners { get; set; }
        public int AwayTeamCorners { get; set; }

        public int HomeTeamYCards { get; set; }
        public int AwayTeamYCards { get; set; }
        public int HomeTeamRedCards { get; set; }
        public int AwayTeamRedCards { get; set; }

        public int HomeTeamOffsides { get; set; }
        public int AwayTeamOffsides { get; set; }

        public virtual Game Game { get; set; }

        public void Copy(GameStats gameStats)
        {
            HalfTimeHomeGoals = gameStats.HalfTimeHomeGoals;
            HalfTimeAwayGoals = gameStats.HalfTimeAwayGoals;
            HalfTimeResult = gameStats.HalfTimeResult;

            HomeTeamShots = gameStats.HomeTeamShots;
            AwayTeamShots = gameStats.AwayTeamShots;

            HomeTeamTargetShots = gameStats.HomeTeamTargetShots;
            AwayTeamTargetShots = gameStats.AwayTeamTargetShots;

            HomeTeamFouls = gameStats.HomeTeamFouls;
            AwayTeamFouls = gameStats.AwayTeamFouls;

            HomeTeamCorners = gameStats.HomeTeamCorners;
            AwayTeamCorners = gameStats.AwayTeamCorners;

            HomeTeamYCards = gameStats.HomeTeamYCards;
            AwayTeamYCards = gameStats.AwayTeamYCards;
            HomeTeamRedCards = gameStats.HomeTeamRedCards;
            AwayTeamRedCards = gameStats.AwayTeamRedCards;

            HomeTeamOffsides = gameStats.HomeTeamOffsides;
            AwayTeamOffsides = gameStats.AwayTeamOffsides;
        }
    }
}

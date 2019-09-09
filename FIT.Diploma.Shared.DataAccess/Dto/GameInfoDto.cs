using System;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class GameInfoDto : GameDto
    {
        //GameStats fields
        public int HalfTimeHomeGoals { get; set; }
        public int HalfTimeAwayGoals { get; set; }
        public GameResultDto HalfTimeResult { get; set; }

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
    }
}

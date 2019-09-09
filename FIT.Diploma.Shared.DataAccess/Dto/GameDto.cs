using System;
using System.Runtime.Serialization;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class GameDto
    {
        public int GameId { get; set; }
        
        public int RoundId { get; set; }
        
        public int HomeTeamId { get; set; }

        public string HomeTeamName { get; set; }

        public int AwayTeamId { get; set; }

        public string AwayTeamName { get; set; }

        public DateTime Date { get; set; }

        public string Stadium { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public GameResultDto Result { get; set; }

        public void Copy(GameDto game)
        {
            if (GameId != game.GameId)
                return;

            RoundId = game.RoundId;
            HomeTeamId = game.HomeTeamId;
            HomeTeamName = game.HomeTeamName;
            AwayTeamId = game.AwayTeamId;
            AwayTeamName = game.AwayTeamName;
            Date = game.Date;
            Stadium = game.Stadium;
            HomeTeamGoals = game.HomeTeamGoals;
            AwayTeamGoals = game.AwayTeamGoals;
            Result = game.Result;
        }
    }

    [Serializable]
    public enum GameResultDto
    {
        HomeWin = 1,
        Draw = 2,
        AwayWin = 3
    }
}

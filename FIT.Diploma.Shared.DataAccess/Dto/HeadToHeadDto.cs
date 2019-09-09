using System;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class HeadToHeadDto
    {
        public int Team1Id { get; set; }
        public string Team1Name { get; set; }
        public int Team2Id { get; set; }
        public string Team2Name { get; set; }

        public int GamePlayed { get; set; }
        public int Team1WinsCount { get; set; }
        public int DrawsCount { get; set; }
        public int Team2WinsCount { get; set; }

        public double Team1WinsPercentage { get; set; }
        public double DrawsPercentage { get; set; }
        public double Team2WinsPercentage { get; set; }

        public int Goals { get; set; }
        public double GoalsPerGame { get; set; }
        public int Team1Goals { get; set; }
        public double Team1GoalsPerGame { get; set; }
        public int Team2Goals { get; set; }
        public double Team2GoalsPerGame { get; set; }


        public int GamesOver2_5 { get; set; }
        public int GamesUnder2_5 { get; set; }

        public int BTTS_Yes { get; set; }
        public int BTTS_No { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class BttsTableDto
    {
        public int LeagueSeasonId { get; set; }
        public List<SeasonTeamBttsDto> Teams { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsFinished { get; set; }
    }

    [Serializable]
    public class SeasonTeamBttsDto
    {
        public int FootballTeamId { get; set; }
        public string FootballTeamName { get; set; }

        public int GamePlayed { get; set; }

        public int BttsYes { get; set; }
        public int BttsNo { get; set; }

        public int BttsYesHome { get; set; }
        public int BttsNoHome { get; set; }

        public int BttsYesAway { get; set; }
        public int BttsNoAway { get; set; }
    }
}

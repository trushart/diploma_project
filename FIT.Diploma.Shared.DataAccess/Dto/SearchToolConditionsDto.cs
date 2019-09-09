using System;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class SearchToolConditionsDto
    {
        public int? TeamId { get; set; }

        public ST_GamePlaceDto GamePlace { get; set; }

        public ST_GameResultDto Result { get; set; }

        public bool? BothTeamsScore { get; set; }

        public ST_Total GameTotal { get; set; }

        public ST_TeamTotal TeamTotal { get; set; }

        public SearchTimeRangeDto TimeRange { get; set; }

        public StreakConditionsDto StreakConditions { get; set; }
    }

    public class StreakConditionsDto
    {
        public int NumberOfItems { get; set; }
        public ST_TotalType TotalType { get; set; }
    }

    public enum ST_GamePlaceDto
    {
        NotDefined,
        Home,
        Away
    }

    public enum ST_GameResultDto
    {
        NotDefined,
        Team1Win,
        Draw,
        Team2Win
    }

    public class SearchTimeRangeDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class ST_Total
    {
        public ST_TotalType TotalType { get; set; }
        public int GoalsNumber { get; set; }
    }

    public class ST_TeamTotal : ST_Total
    {
        public ST_Team Team { get; set; }
    }

    public enum ST_Team
    {
        Team1,
        Team2
    }

    public enum ST_TotalType
    {
        NotDefined,
        Over,
        Equal,        
        Under
    }
}

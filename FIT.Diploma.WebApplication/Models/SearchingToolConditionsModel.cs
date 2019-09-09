using System.Collections.Generic;

namespace FIT.Diploma.WebApplication.Models
{
    public class SearchingToolConditionsModel
    {
        public List<Condition> Conditions { get; set; }
    }

    public class Condition
    {
        public ConditionTypeId ConditionTypeId { get; set; }
        public TotalType TotalType { get; set; }
        public string SelectedCondition { get; set; }

        public string GetTotalType()
        {
            switch (TotalType)
            {
                case TotalType.Over:
                    return "Total Over";
                case TotalType.Equel:
                    return "Equel";
                case TotalType.Under:
                    return "Total Under";
                default:
                    return "Not known";
            }
        }

        public string GetConditionType()
        {
            switch (ConditionTypeId)
            {
                case ConditionTypeId.GameTotal:
                    return "Game Total";
                case ConditionTypeId.TeamTotal:
                    return "Team Total";
                case ConditionTypeId.Btts:
                    return "Both Team To Score";
                case ConditionTypeId.Team:
                    return "Selected Team";
                case ConditionTypeId.GameResult:
                    return "Game Result";
                case ConditionTypeId.GamePlace:
                    return "Game Place";
                default:
                    return "Not known";
            }
        }
    }

    public enum TotalType
    {
        Over = 1,
        Equel = 2,
        Under = 3
    }

    public enum ConditionTypeId
    {
        GameTotal,
        TeamTotal,
        Btts,
        Team,
        GameResult,
        GamePlace
    }
}
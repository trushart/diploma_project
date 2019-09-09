using System.Collections.Generic;

namespace FIT.Diploma.WebApplication.Models
{
    public class SearchingToolModel
    {
        public List<RangeType> RangeTypes { get; set; }
        public List<ResultFormat> ResultFormats { get; set; }
        public List<Condition> ConditionTypes { get; set; }
        public List<Condition> TotalTypes { get; set; }
    }

    public class RangeType
    {
        public string RangeTypeName { get;set; }
        public RangeTypeId RangeTypeId { get; set; }
    }

    public enum RangeTypeId
    {
        DateTime = 1
    }

    public class ResultFormat
    {
        public string ResultFormatName { get; set; }
        public ResultFormatId ResultFormatId { get; set; }
    }

    public enum ResultFormatId
    {
        AllGames = 0,
        NumberOfAllGames = 1,
        MaxStreak = 2,
        MinStreak = 3,
        NumberOfStreaks = 4
    }
}
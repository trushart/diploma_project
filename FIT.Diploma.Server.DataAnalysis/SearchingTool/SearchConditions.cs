namespace FIT.Diploma.Server.DataAnalysis.SearchingTool
{
    public class SearchConditions
    {
        public int? TeamId { get; set; }

        public GamePlace GamePlace { get; set; }

        public GameResult Result { get; set; }

        public bool? BothTeamsScore { get; set; }

        public Total GameTotal { get; set; }

        public TeamTotal TeamTotal { get; set; }

        public SearchTimeRange TimeRange { get; set; }
    }

    public class StreakConditions
    {
        public int NumberOfItems { get; set; }
        public TotalType TotalType { get; set; }
    }

    public enum GamePlace
    {
        NotDefined,
        Home,
        Away        
    }

    public enum GameResult
    {
        NotDefined,
        Team1Win,
        Draw,
        Team2Win
    }
}

namespace FIT.Diploma.Server.DataAnalysis.SearchingTool
{
    public class Total
    {
        public TotalType TotalType { get; set; }
        public int GoalsNumber { get; set; }
    }

    public class TeamTotal : Total
    {
        public Team Team { get; set; }
    }

    public enum Team
    {
        Team1,
        Team2
    }

    public enum TotalType
    {
        NotDefined,
        Under,
        Equal,
        Over
    }
}

namespace FIT.Diploma.Shared.Constants
{
    public class Routes
    {
        //LeagueModule
        public static string AllLeagues => "/leagues/info/all";
        public static string AllLeagueSeasons => "/leagues/{leagueId}/allseasons";
        public static string LeagueInfo => "/leagues/info/{leagueId}";

        //LeagueRoundModule
        public static string SeasonAllRounds => "/seasons/{seasonId}/allrounds";
        public static string RoundGames => "/seasons/{seasonId}/round/{roundId}/games";
        public static string CurrentRoundGames => "/seasons/{seasonId}/round/games";
        public static string SeasonRoundsTable => "/seasons/{seasonId}/round/table";
        public static string SeasonRoundsStats => "/seasons/{seasonId}/round/stats";

        //LeagueSeasonModule
        public static string StandingTable => "/seasons/{seasonId}/standingtable";
        public static string SeasonGames => "/seasons/{seasonId}/allgames";
        public static string GameStats => "/seasons/{seasonId}/matchstats/{id}";
        public static string SeasonTotalStats => "/seasons/{seasonId}/totalstats/table";
        public static string SeasonBttsStats => "/seasons/{seasonId}/btts/table";

        //SearchingToolModule
        public static string SearchingToolGames => "/searching/games";
        public static string SearchingToolGamesNumber => "/searching/games/number";
        public static string SearchingToolMaxStreak => "/searching/streak/maximal";
        public static string SearchingToolMinStreak => "/searching/streak/minimal";
        public static string SearchingToolStreakNumber => "/searching/streak/number";

        //TeamModule
        public static string SeasonTeams => "/seasons/{seasonId}/team/all";
        public static string TeamInfo => "/seasons/{seasonId}/team/{teamId}";
        public static string TeamFormByType => "/seasons/{seasonId}/team/{teamId}/form/{type}";
        public static string TeamForm5 => "/seasons/{seasonId}/team/{teamId}/form5";
        public static string TeamForm10 => "/seasons/{seasonId}/team/{teamId}/form10";
        public static string HeadToHeadStats => "/seasons/{seasonId}/h2h/{team1Id}/{team2Id}";

        //Predictions
        public static string CurrentPredictions => "/seasons/{seasonId}/predictions";
        public static string AllPredictions => "/seasons/{seasonId}/predictions/all";
        public static string FinishedPredictions => "/seasons/{seasonId}/predictions/finished";
    }
}

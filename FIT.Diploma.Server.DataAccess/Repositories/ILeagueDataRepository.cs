using System;
using FIT.Diploma.Server.Database.LeagueData;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataAccess.Repositories
{
    public interface ILeagueDataRepository
    {
        Location GetSpainLocation();
        FootballLeague GetLaLiga();
        void DeleteGameStats(int gameId);
        LeagueSeason CreateAndSaveLeagueSeason(int startYear, FootballLeague league);
        List<LeagueSeason> GetAllLeagueSeasons(int leagueId);
        List<LeagueSeason> GetAllLeagueSeasons(FootballLeague league);
        List<SeasonRound> GetCurrentSeasonRounds(int leagueSeasonId);
        void UpdateSeasonRound(SeasonRound round);
        void RemoveSeasonRound(int roundId);
        void UpdateLeagueSeason(LeagueSeason leagueSeason);
        SeasonRound GetCurrentSeasonRound(LeagueSeason leagueSeason);
        int GetSeasonRoundCount(int leagueSeasonId);
        FootballTeam GetFootballTeam(string name, LeagueSeason season, string sourceId = "");
        bool IsGameExist(FootballTeam homeTeam, FootballTeam awayTeam, DateTime matchDate);
        Game GetGame(FootballTeam homeTeam, FootballTeam awayTeam, DateTime matchDate, LeagueSeason leagueSeason, out bool onlyCreated);
        Game GetGame(string homeTeamName, string awayTeamName, DateTime matchDate, out bool onlyCreated, string source = "");
        List<Game> GetAllRoundGames(int roundId);
        List<Game> GetAllSeasonGames(int seasonId);
        List<Game> GetAllUpcomingGames(int leagueId = -1);
        List<Game> GetTeamsHistory(int teamId1, int teamId2);
        List<Game> GetAllGamesInTimeRange(DateTime fromDate, DateTime toDate, int leagueId = -1);
        void DeleteGameRecord(int gameId);
        void UpdateGameRecord(Game game);
        GameStats GetGameStats(int gameId, out bool onlyCreated);
        void UpdateGameStats(GameStats gameStats);
        DateTime? GetNextUpcomingGameDateTime(int leagueId = -1);
    }
}

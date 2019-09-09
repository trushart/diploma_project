using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIT.Diploma.Shared.DataAccess.Dto;

namespace FIT.Diploma.REST.Client
{
    public interface IRestClient
    {
        List<LeagueDto> GetAllLeagues();
        List<LeagueSeasonDto> GetAllLeagueSeasons(int leagueId);
        List<RoundDto> GetSeasonRounds(int seasonId);
        List<GameDto> GetCurrentRoundGames(int seasonId);
        List<GameDto> GetRoundGames(int seasonId, int roundId);
        StandingTableDto GetStandingTable(int seasonId);

        //stats
        GameInfoDto GetGameStats(int seasonId, int gameId);
        HeadToHeadDto GetH2HStats(int seasonId, int team1Id, int team2Id);

        //Searching tool
        List<GameDto> GetAllGames(SearchToolConditionsDto conditions);
        int GetNumberAllGames(SearchToolConditionsDto conditions);
        List<GameDto> GetMinStreak(SearchToolConditionsDto conditions);
        List<GameDto> GetMaxStreak(SearchToolConditionsDto conditions);
        int GetNumberOfStreak(SearchToolConditionsDto conditions);

        //predictions
        PredictionsResponseDto GetCurrentSeasonPredictions(int seasonId);
        PredictionsResponseDto GetAllSeasonPredictions(int seasonId);
        PredictionsResponseDto GetFinishedSeasonPredictions(int seasonId);
    }
}

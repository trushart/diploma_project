using FIT.Diploma.Server.Database.LeagueData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataAnalysis.SearchingTool
{
    public interface ISearchEngine
    {
        List<Game> GetGames(SearchConditions conditions);

        int GetGamesNumber(SearchConditions conditions);

        List<Game> GetMaximalStreak(SearchConditions conditions);

        List<Game> GetMinimalStreak(SearchConditions conditions);

        int GetNumberOfStreaks(SearchConditions conditions, StreakConditions streakConditions);
    }
}

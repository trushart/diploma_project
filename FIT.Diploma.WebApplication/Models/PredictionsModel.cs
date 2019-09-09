using FIT.Diploma.Shared.DataAccess.Dto;
using System.Collections.Generic;

namespace FIT.Diploma.WebApplication.Models
{
    public class PredictionsModel
    {
        public string Error { get; set; }

        public List<LeagueDto> Leagues { get; set; }
        public LeagueDto SelectedLeague { get; set; }

        public List<LeagueSeasonDto> Seasons { get; set; }
        public LeagueSeasonDto SelectedSeason { get; set; }

        public List<PredictionDto> Predictions { get; set; }
        public PredictionsType SelectedPredictionsType { get; set; }

        public Dictionary<int, string> GetAllPredictionsTypes()
        {
            var results = new Dictionary<int, string>();
            results[(int)PredictionsType.All] = "All Predictions";
            results[(int)PredictionsType.Current] = "Current Predictions";
            results[(int)PredictionsType.Finished] = "Finished Predictions";
            return results;
        }
    }

    public enum PredictionsType
    {
        Current,
        All,
        Finished
    }
}
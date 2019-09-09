using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIT.Diploma.Shared.DataAccess.Dto;
using System.Net.Http;
using System.Net.Http.Headers;
using FIT.Diploma.Shared.Constants;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace FIT.Diploma.REST.Client
{
    public class RestClient : IRestClient
    {
        public static string RestApiAddress = "http://localhost:8888/";

        public List<LeagueDto> GetAllLeagues()
        {
            var results = GetRequestAsync<List<LeagueDto>>(Routes.AllLeagues).GetAwaiter().GetResult();
            return results;
        }

        public List<LeagueSeasonDto> GetAllLeagueSeasons(int leagueId)
        {
            var endpoint = UpdateEndPointWithParams(Routes.AllLeagueSeasons, leagueId.ToString());
            var results = GetRequestAsync<List<LeagueSeasonDto>>(endpoint).GetAwaiter().GetResult();
            return results;
        }

        public List<RoundDto> GetSeasonRounds(int seasonId)
        {
            var endpoint = UpdateEndPointWithParams(Routes.SeasonAllRounds, seasonId.ToString());
            var results = GetRequestAsync<List<RoundDto>>(endpoint).GetAwaiter().GetResult();
            return results;
        }

        public List<GameDto> GetCurrentRoundGames(int seasonId)
        {
            var endpoint = UpdateEndPointWithParams(Routes.CurrentRoundGames, seasonId.ToString());
            var results = GetRequestAsync<List<GameDto>>(endpoint).GetAwaiter().GetResult();
            return results;
        }

        public List<GameDto> GetRoundGames(int seasonId, int roundId)
        {
            var endpoint = UpdateEndPointWithParams(Routes.RoundGames, seasonId.ToString(), roundId.ToString());
            var results = GetRequestAsync<List<GameDto>>(endpoint).GetAwaiter().GetResult();
            return results;
        }

        public StandingTableDto GetStandingTable(int seasonId)
        {
            var endpoint = UpdateEndPointWithParams(Routes.StandingTable, seasonId.ToString());
            var results = GetRequestAsync<StandingTableDto>(endpoint).GetAwaiter().GetResult();
            return results;
        }

        private static string UpdateEndPointWithParams(string endpoint, params string[] parameters)
        {
            var numberOfParams = parameters?.Length ?? 0;
            var regex = new Regex("{[a-zA-Z0-9]+}");
            if (regex.Matches(endpoint).Count != numberOfParams)
                throw new Exception("Number of parameters not matches");
            if (numberOfParams == 0)
                return endpoint;
            foreach (var param in parameters)
            {
                endpoint = regex.Replace(endpoint, param, 1);
            }
            return endpoint;
        }

        private async Task<TResult> GetRequestAsync<TResult>(string endpoint)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(RestApiAddress)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(responseJson);
                return result;
            }
            else
            {
                throw new Exception($"RestClient got non-success status code [{response.StatusCode}]. Reason: {response.ReasonPhrase}. Response message :{response.Content}");
            }
        }

        private async Task<TResult> PostRequestAsync<TResult>(string endpoint, object data)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(RestApiAddress)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync(endpoint, byteContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(responseJson);
                return result;
            }
            else
            {
                throw new Exception($"RestClient got non-success status code [{response.StatusCode}]. Reason: {response.ReasonPhrase}. Response message :{response.Content}");
            }
        }


        //Searching Tool
        public List<GameDto> GetAllGames(SearchToolConditionsDto conditions)
        {
            var results = PostRequestAsync<List<GameDto>>(Routes.SearchingToolGames, conditions).GetAwaiter().GetResult();
            return results;
        }

        public int GetNumberAllGames(SearchToolConditionsDto conditions)
        {
            var results = PostRequestAsync<int>(Routes.SearchingToolGamesNumber, conditions).GetAwaiter().GetResult();
            return results;
        }

        public List<GameDto> GetMinStreak(SearchToolConditionsDto conditions)
        {
            var results = PostRequestAsync<List<GameDto>>(Routes.SearchingToolMinStreak, conditions).GetAwaiter().GetResult();
            return results;
        }

        public List<GameDto> GetMaxStreak(SearchToolConditionsDto conditions)
        {
            var results = PostRequestAsync<List<GameDto>>(Routes.SearchingToolMaxStreak, conditions).GetAwaiter().GetResult();
            return results;
        }

        public int GetNumberOfStreak(SearchToolConditionsDto conditions)
        {
            var results = PostRequestAsync<int>(Routes.SearchingToolStreakNumber, conditions).GetAwaiter().GetResult();
            return results;
        }

        public PredictionsResponseDto GetCurrentSeasonPredictions(int seasonId)
        {
            var endpoint = UpdateEndPointWithParams(Routes.CurrentPredictions, seasonId.ToString());
            var results = GetRequestAsync<PredictionsResponseDto>(endpoint).GetAwaiter().GetResult();
            return results;
        }

        public PredictionsResponseDto GetAllSeasonPredictions(int seasonId)
        {
            var endpoint = UpdateEndPointWithParams(Routes.AllPredictions, seasonId.ToString());
            var results = GetRequestAsync<PredictionsResponseDto>(endpoint).GetAwaiter().GetResult();
            return results;
        }

        public PredictionsResponseDto GetFinishedSeasonPredictions(int seasonId)
        {
            var endpoint = UpdateEndPointWithParams(Routes.FinishedPredictions, seasonId.ToString());
            var results = GetRequestAsync<PredictionsResponseDto>(endpoint).GetAwaiter().GetResult();
            return results;
        }

        public GameInfoDto GetGameStats(int seasonId, int gameId)
        {
            var endpoint = UpdateEndPointWithParams(Routes.GameStats, seasonId.ToString(), gameId.ToString());
            var results = GetRequestAsync<GameInfoDto>(endpoint).GetAwaiter().GetResult();
            return results;
        }

        public HeadToHeadDto GetH2HStats(int seasonId, int team1Id, int team2Id)
        {
            var endpoint = UpdateEndPointWithParams(Routes.HeadToHeadStats, seasonId.ToString(), team1Id.ToString(), team2Id.ToString());
            var results = GetRequestAsync<HeadToHeadDto>(endpoint).GetAwaiter().GetResult();
            return results;
        }
    }
}

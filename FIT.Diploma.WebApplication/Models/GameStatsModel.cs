using System.Collections.Generic;
using FIT.Diploma.Shared.DataAccess.Dto;

namespace FIT.Diploma.WebApplication.Models
{
    public class GameStatsModel
    {
        public GameInfoDto GameInfo { get; set; }
        public HeadToHeadDto HeadToHead { get; set; }

        //public List<GameDto> AllGames { get; set; }
    }
}
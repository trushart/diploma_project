using System.Collections.Generic;
using FIT.Diploma.Shared.DataAccess.Dto;

namespace FIT.Diploma.WebApplication.Models
{
    public class SearchingToolResults
    {
        public int GamesNumber { get; set; }
        public int StreaksNumber { get; set; }

        public List<GameDto> Games { get; set; }
    }
}
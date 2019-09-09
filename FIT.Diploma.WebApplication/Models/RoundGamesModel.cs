using System.Collections.Generic;
using FIT.Diploma.Shared.DataAccess.Dto;

namespace FIT.Diploma.WebApplication.Models
{
    public class RoundGamesModel
    {
        public string TableHeader { get; set; }
        public List<GameDto> Games { get; set; }
        public RoundDto Round { get; set; }
    }
}
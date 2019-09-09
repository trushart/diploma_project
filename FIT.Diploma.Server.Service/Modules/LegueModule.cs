using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace FIT.Diploma.Server.Service.Modules
{
    public class LeagueModule : NancyModule
    {
        public LeagueModule() : base("/leagues")
        {
            Get["/info/all"] = _ => {
                List<LeagueModule> league;
                return "The list of products";
            };
        }
    }
}

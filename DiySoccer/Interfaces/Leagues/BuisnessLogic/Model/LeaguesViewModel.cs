using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeaguesViewModel
    {
        [JsonProperty("leagues")]
        public IEnumerable<LeagueViewModel> Leagues { get; set; }

        [JsonProperty("tournaments")]
        public IEnumerable<LeagueViewModel> Tournaments { get; set; }

        public LeaguesViewModel()
        {
            Leagues = Enumerable.Empty<LeagueViewModel>();
            Tournaments = Enumerable.Empty<LeagueViewModel>();
        }
    }
}

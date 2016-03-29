using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueStatisticViewModel
    {
        [JsonProperty("bestHelpers")]
        public IEnumerable<IdNameViewModel> BestHelpers { get; set; }

        [JsonProperty("bestForwards")]
        public IEnumerable<IdNameViewModel> BestForwards { get; set; }

        [JsonProperty("bestPlayers")]
        public IEnumerable<IdNameViewModel> BestPlayers { get; set; }

        [JsonProperty("teamStats")]
        public IEnumerable<TeamStatisticViewModel> TeamStatistics { get; set; }

        public LeagueStatisticViewModel()
        {
            TeamStatistics = Enumerable.Empty<TeamStatisticViewModel>();
            BestHelpers = Enumerable.Empty<IdNameViewModel>();
            BestForwards = Enumerable.Empty<IdNameViewModel>();
            BestPlayers = Enumerable.Empty<IdNameViewModel>();
        }
    }
}

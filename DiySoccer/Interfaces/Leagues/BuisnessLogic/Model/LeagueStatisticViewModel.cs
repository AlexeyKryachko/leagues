using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueStatisticViewModel
    {
        [JsonProperty("bestHelpers")]
        public IEnumerable<LeagueMemberStatisticViewModel> BestHelpers { get; set; }

        [JsonProperty("bestForwards")]
        public IEnumerable<LeagueMemberStatisticViewModel> BestForwards { get; set; }

        [JsonProperty("bestPlayers")]
        public IEnumerable<LeagueMemberStatisticViewModel> BestPlayers { get; set; }

        [JsonProperty("teamStats")]
        public IEnumerable<TeamStatisticViewModel> TeamStatistics { get; set; }

        public LeagueStatisticViewModel()
        {
            TeamStatistics = Enumerable.Empty<TeamStatisticViewModel>();
            BestHelpers = Enumerable.Empty<LeagueMemberStatisticViewModel>();
            BestForwards = Enumerable.Empty<LeagueMemberStatisticViewModel>();
            BestPlayers = Enumerable.Empty<LeagueMemberStatisticViewModel>();
        }
    }
}

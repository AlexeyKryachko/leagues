using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueStatisticsViewModel
    {
        [JsonProperty("bestHelpers")]
        public IEnumerable<LeagueMemberStatisticViewModel> BestHelpers { get; set; }

        [JsonProperty("bestForwards")]
        public IEnumerable<LeagueMemberStatisticViewModel> BestForwards { get; set; }

        [JsonProperty("bestPlayers")]
        public IEnumerable<LeagueMemberStatisticViewModel> BestPlayers { get; set; }

        public LeagueStatisticsViewModel()
        {
            BestHelpers = Enumerable.Empty<LeagueMemberStatisticViewModel>();
            BestForwards = Enumerable.Empty<LeagueMemberStatisticViewModel>();
            BestPlayers = Enumerable.Empty<LeagueMemberStatisticViewModel>();
        }
    }
}

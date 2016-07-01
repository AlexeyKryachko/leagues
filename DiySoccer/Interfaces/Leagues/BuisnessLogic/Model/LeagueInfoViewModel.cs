using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueInfoViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("mediaId")]
        public string MediaId { get; set; }
        [JsonProperty("teams")]
        public IEnumerable<LeagueInfoTeamViewModel> Teams { get; set; }
        [JsonProperty("fEvents")]
        public IEnumerable<LeagueInfoEventViewModel> FutureEvents { get; set; }
        [JsonProperty("pEvents")]
        public IEnumerable<LeagueInfoEventViewModel> PastEvents { get; set; }
        [JsonProperty("bestPlayer")]
        public IdNameViewModel BestPlayer { get; set; }
        [JsonProperty("bestGoalPlayer")]
        public IdNameViewModel BestGoalPlayer { get; set; }
        [JsonProperty("bestHelpPlayer")]
        public IdNameViewModel BestHelpPlayer { get; set; }
        [JsonProperty("News")]
        public IEnumerable<IdNameViewModel> News { get; set; }

        public LeagueInfoViewModel()
        {
            Teams = Enumerable.Empty<LeagueInfoTeamViewModel>();
            FutureEvents = Enumerable.Empty<LeagueInfoEventViewModel>();
            PastEvents = Enumerable.Empty<LeagueInfoEventViewModel>();
            News = Enumerable.Empty<IdNameViewModel>();
        }
    }
}

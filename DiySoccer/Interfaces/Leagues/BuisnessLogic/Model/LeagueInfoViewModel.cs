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

        [JsonProperty("subName")]
        public string SubName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("mediaId")]
        public string MediaId { get; set; }
        [JsonProperty("teams")]
        public IEnumerable<LeagueInfoTeamViewModel> Teams { get; set; }
        [JsonProperty("events")]
        public IEnumerable<LeagueInfoEventViewModel> Events { get; set; }
        [JsonProperty("gamesToPlay")]
        public IEnumerable<LeagueInfoGameToPlayViewModel> GamesToPlay { get; set; }
        [JsonProperty("bestPlayer")]
        public LeagueInfoPlayerViewModel BestPlayer { get; set; }
        [JsonProperty("bestGoalPlayer")]
        public LeagueInfoPlayerViewModel BestGoalPlayer { get; set; }
        [JsonProperty("bestHelpPlayer")]
        public LeagueInfoPlayerViewModel BestHelpPlayer { get; set; }
        [JsonProperty("News")]
        public IEnumerable<IdNameViewModel> News { get; set; }

        public LeagueInfoViewModel()
        {
            Teams = Enumerable.Empty<LeagueInfoTeamViewModel>();
            Events = Enumerable.Empty<LeagueInfoEventViewModel>();
            GamesToPlay = Enumerable.Empty<LeagueInfoGameToPlayViewModel>();
            News = Enumerable.Empty<IdNameViewModel>();
        }
    }
}

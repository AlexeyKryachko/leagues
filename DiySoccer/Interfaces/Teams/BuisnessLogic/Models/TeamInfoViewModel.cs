using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Teams.BuisnessLogic.Models
{
    public class TeamInfoViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mediaId")]
        public string MediaId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("games")]
        public IEnumerable<TeamInfoGameViewModel> Games { get; set; }

        [JsonProperty("players")]
        public IEnumerable<TeamInfoMemberViewModel> Players { get; set; }

        [JsonProperty("stats")]
        public IEnumerable<TeamInfoStatisticViewModel> Statistics { get; set; }

        [JsonProperty("playersStats")]
        public IEnumerable<TeamInfoPlayersStatisticViewModel> PlayersStatistics { get; set; }

        public TeamInfoViewModel()
        {
            Games = Enumerable.Empty<TeamInfoGameViewModel>();
            Players = Enumerable.Empty<TeamInfoMemberViewModel>();
            Statistics = Enumerable.Empty<TeamInfoStatisticViewModel>();
        }
    }
}

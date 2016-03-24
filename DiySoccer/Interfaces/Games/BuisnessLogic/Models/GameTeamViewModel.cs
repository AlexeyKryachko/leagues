using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class GameTeamViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("bestId")]
        public string BestId { get; set; }

        [JsonProperty("members")]
        public IEnumerable<GameMemberViewModel> Members { get; set; }

        public GameTeamViewModel()
        {
            Members = Enumerable.Empty<GameMemberViewModel>();
        }
    }
}

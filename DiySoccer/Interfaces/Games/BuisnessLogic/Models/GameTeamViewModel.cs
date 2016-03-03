using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class GameTeamViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("members")]
        public IEnumerable<GameMemberViewModel> Members { get; set; }
    }
}

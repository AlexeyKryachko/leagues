using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class GameMemberViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("help")]
        public int Help { get; set; }
    }
}

using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class GameInfoMemberScoreViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("help")]
        public int Help { get; set; }
    }
}

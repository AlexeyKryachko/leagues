using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class GameVewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("customScores")]
        public bool CustomScores { get; set; }

        [JsonProperty("homeTeam")]
        public GameTeamViewModel HomeTeam { get; set; }

        [JsonProperty("guestTeam")]
        public GameTeamViewModel GuestTeam { get; set; }
    }
}

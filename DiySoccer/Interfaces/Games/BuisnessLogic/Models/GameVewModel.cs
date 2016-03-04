using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class GameVewModel
    {
        [JsonProperty("leagueId")]
        public string LeagueId { get; set; }

        [JsonProperty("customScores")]
        public bool CustomScores { get; set; }

        [JsonProperty("homeTeam")]
        public GameTeamViewModel HomeTeam { get; set; }

        [JsonProperty("guestTeam")]
        public GameTeamViewModel GuestTeam { get; set; }
    }
}

using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueInfoTeamViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mediaId")]
        public string MediaId { get; set; }

        [JsonProperty("games")]
        public int Games { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonIgnore]
        public int Scores { get; set; }

        [JsonIgnore]
        public int Missed { get; set; }
    }
}

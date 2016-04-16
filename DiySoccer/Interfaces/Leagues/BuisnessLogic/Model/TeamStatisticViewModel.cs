using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class TeamStatisticViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gamesCount")]
        public int GamesCount { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("loses")]
        public int Loses { get; set; }

        [JsonProperty("draw")]
        public int Draws { get; set; }

        [JsonProperty("scores")]
        public int Scores { get; set; }

        [JsonProperty("missed")]
        public int Missed { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("mediaId")]
        public string MediaId { get; set; }
    }
}

using Newtonsoft.Json;

namespace Interfaces.Teams.BuisnessLogic.Models
{
    public class TeamInfoGameViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string OpponentName { get; set; }

        [JsonProperty("goals")]
        public string Goals { get; set; }
    }
}

using Newtonsoft.Json;

namespace Interfaces.Teams.BuisnessLogic.Models
{
    public class TeamInfoGameViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("goals")]
        public string Goals { get; set; }
    }
}

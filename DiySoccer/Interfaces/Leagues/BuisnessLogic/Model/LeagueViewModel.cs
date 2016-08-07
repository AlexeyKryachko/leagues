using Interfaces.Leagues.DataAccess.Model;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public LeagueType Type { get; set; }

        [JsonProperty("mediaId")]
        public string MediaId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}

using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueMemberStatisticViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("num")]
        public int Number { get; set; }
    }
}

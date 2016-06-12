using Newtonsoft.Json;

namespace Interfaces.Teams.BuisnessLogic.Models
{
    public class TeamInfoPlayersStatisticViewModel
    {
        [JsonProperty("num")]
        public string Number { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("goal")]
        public int Goal { get; set; }

        [JsonProperty("help")]
        public int Help { get; set; }

        [JsonProperty("sum")]
        public int Sum { get; set; }

        [JsonProperty("best")]
        public int Best { get; set; }
    }
}

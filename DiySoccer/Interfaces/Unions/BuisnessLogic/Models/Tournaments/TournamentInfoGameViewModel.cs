using Newtonsoft.Json;

namespace Interfaces.Unions.BuisnessLogic.Models.Tournaments
{
    public class TournamentInfoGameViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }
}

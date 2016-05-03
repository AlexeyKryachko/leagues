using Newtonsoft.Json;

namespace Interfaces.Events.BuisnessLogic.Models
{
    public class EventGameVewModel
    {
        [JsonProperty("homeTeamName")]
        public string HomeTeamName { get; set; }

        [JsonProperty("homeTeamId")]
        public string HomeTeamId { get; set; }

        [JsonProperty("guestTeamName")]
        public string GuestTeamName { get; set; }

        [JsonProperty("guestTeamId")]
        public string GuestTeamId { get; set; }
    }
}

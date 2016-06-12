using System.Collections.Generic;
using Interfaces.Core;
using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class GameVewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("eventId")]
        public string EventId { get; set; }

        [JsonProperty("events")]
        public IEnumerable<IdNameViewModel> Events { get; set; }

        [JsonProperty("customScores")]
        public bool CustomScores { get; set; }

        [JsonProperty("homeTeam")]
        public GameTeamViewModel HomeTeam { get; set; }

        [JsonProperty("guestTeam")]
        public GameTeamViewModel GuestTeam { get; set; }
    }
}

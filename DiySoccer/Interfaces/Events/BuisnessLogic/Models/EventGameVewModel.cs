﻿using Newtonsoft.Json;

namespace Interfaces.Events.BuisnessLogic.Models
{
    public class EventGameVewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("homeTeamId")]
        public string HomeTeamId { get; set; }
        
        [JsonProperty("guestTeamId")]
        public string GuestTeamId { get; set; }

        [JsonProperty("gameId")]
        public string GameId { get; set; }
    }
}

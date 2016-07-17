﻿using Newtonsoft.Json;

namespace Interfaces.Unions.BuisnessLogic.Models.Tournaments
{
    public class TournamentInfoGroupGameViewModel : ITournamentInfoGroupViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("drafts")]
        public int Drafts { get; set; }

        [JsonProperty("loses")]
        public int Loses { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }
    }
}

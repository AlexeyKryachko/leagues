using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueInfoGameToPlayViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("important")]
        public bool Important { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("teams")]
        public IEnumerable<string> Teams { get; set; }
    }
}

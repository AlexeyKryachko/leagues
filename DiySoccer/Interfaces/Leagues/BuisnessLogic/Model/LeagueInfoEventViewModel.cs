using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueInfoEventViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("teams")]
        public IEnumerable<string> Teams { get; set; }
    }
}

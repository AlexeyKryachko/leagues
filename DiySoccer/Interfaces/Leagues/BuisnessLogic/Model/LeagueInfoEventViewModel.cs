using System;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueInfoEventViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("date")]
        public DateTime? Date { get; set; }
    }
}

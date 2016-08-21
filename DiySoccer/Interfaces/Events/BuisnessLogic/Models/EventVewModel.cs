using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Events.BuisnessLogic.Models
{
    public class EventVewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("minor")]
        public bool Minor { get; set; }

        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("games")]
        public IEnumerable<EventGameVewModel> Games { get; set; }

        public EventVewModel()
        {
            Games = Enumerable.Empty<EventGameVewModel>();
        }
    }
}

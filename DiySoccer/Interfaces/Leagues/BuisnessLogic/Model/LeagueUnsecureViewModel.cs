using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueUnsecureViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("vkGroup")]
        public string VkGroup { get; set; }
        
        [JsonProperty("Admins")]
        public IEnumerable<string> Admins { get; set; }

        public LeagueUnsecureViewModel()
        {
            Admins = Enumerable.Empty<string>();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
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

        [JsonProperty("admins")]
        public IEnumerable<IdNameViewModel> Admins { get; set; }

        public LeagueUnsecureViewModel()
        {
            Admins = Enumerable.Empty<IdNameViewModel>();
        }
    }
}

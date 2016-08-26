using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Interfaces.Leagues.DataAccess.Model;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueUnsecureViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public LeagueType Type { get; set; }

        [JsonProperty("mediaId")]
        public string MediaId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subName")]
        public string SubName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("information")]
        public string Information { get; set; }

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

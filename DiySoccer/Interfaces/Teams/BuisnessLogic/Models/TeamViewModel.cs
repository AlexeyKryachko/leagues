using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Newtonsoft.Json;

namespace Interfaces.Teams.BuisnessLogic.Models
{
    public class TeamViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("media")]
        public string Media { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("members")]
        public IEnumerable<IdNameViewModel> Members { get; set; }

        public TeamViewModel()
        {
            Members = Enumerable.Empty<IdNameViewModel>();
        }
    }
}

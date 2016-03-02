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

        [JsonProperty("members")]
        public IEnumerable<IdValueViewModel> Members { get; set; }

        public TeamViewModel()
        {
            Members = Enumerable.Empty<IdValueViewModel>();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Newtonsoft.Json;

namespace Interfaces.Teams.BuisnessLogic.Models
{
    public class CreateTeamViewModel
    {
        [JsonProperty("league")]
        public string League { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("members")]
        public IEnumerable<IdValueViewModel> Members { get; set; }

        public CreateTeamViewModel()
        {
            Members = Enumerable.Empty<IdValueViewModel>();
        }
    }
}

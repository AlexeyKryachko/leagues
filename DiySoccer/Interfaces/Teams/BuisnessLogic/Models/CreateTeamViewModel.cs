using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Newtonsoft.Json;

namespace Interfaces.Teams.BuisnessLogic.Models
{
    public class CreateTeamViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("members")]
        public IEnumerable<IdNameViewModel> Members { get; set; }

        public CreateTeamViewModel()
        {
            Members = Enumerable.Empty<IdNameViewModel>();
        }
    }
}

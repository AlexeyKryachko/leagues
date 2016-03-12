using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Teams.BuisnessLogic.Models
{
    public class TeamInfoViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("games")]
        public IEnumerable<TeamInfoGameViewModel> Games { get; set; }

        public TeamInfoViewModel()
        {
            Games = Enumerable.Empty<TeamInfoGameViewModel>();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Unions.BuisnessLogic.Models.Tournaments
{
    public class TournamentInfoViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subName")]
        public string SubName { get; set; }

        [JsonProperty("mediaId")]
        public string MediaId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

		[JsonProperty("information")]
        public string Information { get; set; }

        [JsonProperty("events")]
        public IEnumerable<ITournamentInfoGroupViewModel> Events { get; set; }

        public TournamentInfoViewModel()
        {
            Events = Enumerable.Empty<ITournamentInfoGroupViewModel>();
        }
    }
}

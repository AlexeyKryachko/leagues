using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Unions.BuisnessLogic.Models.Tournaments
{
    public class TournamentInfoViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("events")]
        public IEnumerable<IEnumerable<ITournamentInfoGroupViewModel>> Events { get; set; }

        public TournamentInfoViewModel()
        {
            Events = Enumerable.Empty<IEnumerable<ITournamentInfoGroupViewModel>>();
        }
    }
}

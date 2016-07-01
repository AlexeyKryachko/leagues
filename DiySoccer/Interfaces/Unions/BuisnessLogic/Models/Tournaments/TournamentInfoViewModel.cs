using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Unions.BuisnessLogic.Models.Tournaments
{
    public class TournamentInfoViewModel
    {
        [JsonProperty("events")]
        public IEnumerable<IEnumerable<TournamentInfoGroupViewModel>> Events { get; set; }

        public TournamentInfoViewModel()
        {
            Events = Enumerable.Empty<IEnumerable<TournamentInfoGroupViewModel>>();
        }
    }
}

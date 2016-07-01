using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Unions.BuisnessLogic.Models.Tournaments
{
    public class TournamentInfoGroupViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("teams")]
        public IEnumerable<TournamentInfoGameViewModel> Teams { get; set; }

        public TournamentInfoGroupViewModel()
        {
            Teams = Enumerable.Empty<TournamentInfoGameViewModel>();
        }
    }
}

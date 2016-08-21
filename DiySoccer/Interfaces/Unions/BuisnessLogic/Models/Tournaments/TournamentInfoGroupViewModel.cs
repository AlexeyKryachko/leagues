using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Unions.BuisnessLogic.Models.Tournaments
{
    public class TournamentInfoGroupViewModel : ITournamentInfoGroupViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("minor")]
        public bool Minor { get; set; }

        [JsonProperty("playoffGames")]
        public IEnumerable<TournamentInfoPlayOffGameViewModel> PlayOffGames { get; set; }

        [JsonProperty("groupGames")]
        public IEnumerable<TournamentInfoGroupGameViewModel> GroupGames { get; set; }

        [JsonProperty("games")]
        public IEnumerable<TournamentInfoPlayOffGameViewModel> Games { get; set; }

        public TournamentInfoGroupViewModel()
        {
            PlayOffGames = Enumerable.Empty<TournamentInfoPlayOffGameViewModel>();
            GroupGames = Enumerable.Empty<TournamentInfoGroupGameViewModel>();
            Games = Enumerable.Empty<TournamentInfoPlayOffGameViewModel>();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class GameInfoViewModel
    {
        [JsonProperty("homeTeamName")]
        public string HomeTeamName { get; set; }

        [JsonProperty("guestTeamName")]
        public string GuestTeamName { get; set; }

        [JsonProperty("homeTeamMediaId")]
        public string HomeTeamMediaId { get; set; }

        [JsonProperty("guestTeamMediaId")]
        public string GuestTeamMediaId { get; set; }

        [JsonProperty("homeTeamScore")]
        public int HomeTeamScore { get; set; }

        [JsonProperty("guestTeamScore")]
        public int GuestTeamScore { get; set; }

        [JsonProperty("homeTeamBest")]
        public string HomeTeamBestPlayer { get; set; }

        [JsonProperty("guestTeamBest")]
        public string GuestTeamBestPlayer { get; set; }

        [JsonProperty("homeTeamScores")]
        public IEnumerable<GameInfoMemberScoreViewModel> HomeTeamScores { get; set; }

        [JsonProperty("guestTeamScores")]
        public IEnumerable<GameInfoMemberScoreViewModel> GuestTeamScores { get; set; }

        public GameInfoViewModel()
        {
            HomeTeamScores = Enumerable.Empty<GameInfoMemberScoreViewModel>();
            GuestTeamScores = Enumerable.Empty<GameInfoMemberScoreViewModel>();
        }
    }
}

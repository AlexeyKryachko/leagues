using Newtonsoft.Json;

namespace Interfaces.Unions.BuisnessLogic.Models.Tournaments
{
    public class TournamentInfoPlayOffGameViewModel
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("homeTeamMediaId")]
        public string HomeTeamMediaId { get; set; }

        [JsonProperty("homeTeamName")]
        public string HomeTeamName { get; set; }

        [JsonProperty("homeTeamScore")]
        public int HomeTeamScore { get; set; }

        [JsonProperty("guestTeamMediaId")]
        public string GuestTeamMediaId { get; set; }

        [JsonProperty("guestTeamName")]
        public string GuestTeamName { get; set; }

        [JsonProperty("guestTeamScore")]
        public int GuestTeamScore { get; set; }
    }
}

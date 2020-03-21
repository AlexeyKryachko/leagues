using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interfaces.GameApproval.BuisnessLogic.Model
{
    public class ApprovalViewModel
    {
        [JsonProperty("id")]
        public string Id;
        
        [JsonProperty("eventTitle")]
        public string EventTitle;
        
        [JsonProperty("homeTeamTitle")]
        public string HomeTeamTitle;

        [JsonProperty("homeTeamScore")]
        public int HomeTeamScore;

        [JsonProperty("homePlayers")]
        public IEnumerable<PlayerApprovalViewModel> HomePlayers;

        [JsonProperty("guestTeamTitle")]
        public string GuestTeamTitle;

        [JsonProperty("guestTeamScore")]
        public int GuestTeamScore;

        [JsonProperty("guestPlayers")]
        public IEnumerable<PlayerApprovalViewModel> GuestPlayers;
    }
}

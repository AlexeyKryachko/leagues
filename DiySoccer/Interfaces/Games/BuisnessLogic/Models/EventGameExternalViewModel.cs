using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class EventGameExternalViewModel
    {
        [JsonProperty("title")]
        public string Title;

        [JsonProperty("value")]
        public string Value;

        [JsonProperty("selected")]
        public bool Selected;

        [JsonProperty("homeTeamTitle")]
        public string HomeTeamTitle;

        [JsonProperty("homeTeam")]
        public GameTeamViewModel HomeTeam;

        [JsonProperty("guestTeamTitle")]
        public string GuestTeamTitle;

        [JsonProperty("guestTeam")]
        public GameTeamViewModel GuestTeam;
    }
}

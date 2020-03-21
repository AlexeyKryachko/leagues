using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class EventsGameExternalViewModel
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("title")]
        public string Title;
        [JsonProperty("selected")]
        public bool Selected;
        [JsonProperty("games")]
        public List<EventGameExternalViewModel> Games;

        public EventsGameExternalViewModel()
        {
            Games = new List<EventGameExternalViewModel>();
        }
    }
}

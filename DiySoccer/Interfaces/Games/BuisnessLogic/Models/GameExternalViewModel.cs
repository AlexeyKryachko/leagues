using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Interfaces.Games.BuisnessLogic.Models
{
    public class GameExternalViewModel
    {
        [JsonProperty("events")]
        public IEnumerable<EventsGameExternalViewModel> Events;
    }
}

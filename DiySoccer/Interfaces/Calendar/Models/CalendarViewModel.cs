using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Interfaces.Events.BuisnessLogic.Models;
using Interfaces.Shared;
using Newtonsoft.Json;

namespace Interfaces.Calendar.Models
{
    public class CalendarViewModel : BreadcrumpsViewModel
    {
        [JsonProperty("events")]
        public IEnumerable<EventVewModel> Events { get; set; }

        [JsonProperty("teams")]
        public IEnumerable<IdNameViewModel> Teams { get; set; }

        public CalendarViewModel()
        {
            Events = Enumerable.Empty<EventVewModel>();
            Teams = Enumerable.Empty<IdNameViewModel>();
        }
    }
}

using System.Collections.Generic;
using Interfaces.Calendar.Models;
using Interfaces.Core;

namespace Interfaces.Calendar
{
    public interface ICalendarManager
    {
        CalendarViewModel GetViewModel(string leagueId);

        IEnumerable<IdNameViewModel> GetEvents(string leagueId);
    }
}

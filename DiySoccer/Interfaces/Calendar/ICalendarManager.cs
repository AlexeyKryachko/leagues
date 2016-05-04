using Interfaces.Calendar.Models;

namespace Interfaces.Calendar
{
    public interface ICalendarManager
    {
        CalendarViewModel GetViewModel(string leagueId);
    }
}

using System.Linq;
using Interfaces.Calendar;
using Interfaces.Calendar.Models;
using Interfaces.Core;
using Interfaces.Events.BuisnessLogic;
using Interfaces.Teams.BuisnessLogic;
using Interfaces.Teams.DataAccess;

namespace Implementations.Calendar
{
    public class CalendarManager : ICalendarManager
    {
        private readonly IEventsManager _eventsManager;
        private readonly ITeamsRepository _teamsRepository;

        public CalendarManager(IEventsManager eventsManager, ITeamsRepository teamsRepository)
        {
            _eventsManager = eventsManager;
            _teamsRepository = teamsRepository;
        }

        public CalendarViewModel GetViewModel(string leagueId)
        {
            var events = _eventsManager.GetRange(leagueId);
            var teams = _teamsRepository.GetByLeague(leagueId);

            return new CalendarViewModel
            {
                Events = events,
                Teams = teams.Select(x => new IdNameViewModel
                {
                    Id = x.EntityId,
                    Name = x.Name
                })
            };
        }
    }
}

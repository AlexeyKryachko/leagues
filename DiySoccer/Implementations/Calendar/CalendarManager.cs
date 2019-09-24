using System.Collections.Generic;
using System.Linq;
using Interfaces.Calendar;
using Interfaces.Calendar.Models;
using Interfaces.Core;
using Interfaces.Events.BuisnessLogic;
using Interfaces.Leagues.DataAccess;
using Interfaces.Shared;
using Interfaces.Teams.BuisnessLogic;
using Interfaces.Teams.DataAccess;

namespace Implementations.Calendar
{
    public class CalendarManager : ICalendarManager
    {
        private readonly IEventsManager _eventsManager;
        private readonly ITeamsRepository _teamsRepository;
        private readonly ILeaguesRepository _leaguesRepository;

        public CalendarManager(IEventsManager eventsManager, ITeamsRepository teamsRepository, ILeaguesRepository leaguesRepository)
        {
            _eventsManager = eventsManager;
            _teamsRepository = teamsRepository;
            _leaguesRepository = leaguesRepository;
        }

        public CalendarViewModel GetViewModel(string leagueId)
        {
            var league = _leaguesRepository.Get(leagueId);
            var events = _eventsManager.GetRange(leagueId);
            var teams = _teamsRepository.GetByLeague(leagueId);

            var breadcrumps = new List<BreadcrumpViewModel>
            {
                new BreadcrumpViewModel
                {
                    Id = league.EntityId,
                    Name = league.Name,
                    Type = BreadcrumpType.League
                }
            };

            return new CalendarViewModel
            {
                Breadcrumps = breadcrumps,
                Events = events,
                Teams = teams.Select(x => new IdNameViewModel
                {
                    Id = x.EntityId,
                    Name = x.Name
                })
            };
        }

        public IEnumerable<IdNameViewModel> GetEvents(string leagueId)
        {
            var events = _eventsManager.GetRange(leagueId);

            return events.Select(x => new IdNameViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
        }
    }
}

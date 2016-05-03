using System.Collections.Generic;
using System.Linq;
using Interfaces.Events.BuisnessLogic;
using Interfaces.Events.BuisnessLogic.Models;
using Interfaces.Events.DataAccess;
using Interfaces.Teams.DataAccess;

namespace Implementations.Events.BuisnessLogic
{
    public class EventsManager : IEventsManager
    {
        private readonly ITeamsRepository _teamsRepository;
        private readonly IEventsRepository _eventsRepository;
        private readonly EventsMapper _eventMapper;

        public EventsManager(IEventsRepository eventsRepository, EventsMapper eventMapper, ITeamsRepository teamsRepository)
        {
            _eventsRepository = eventsRepository;
            _eventMapper = eventMapper;
            _teamsRepository = teamsRepository;
        }

        public IEnumerable<EventVewModel> GetRange(string leagueId)
        {
            var events = _eventsRepository.GetByLeague(leagueId);

            var gamesIds = events.SelectMany(x => x.Games);
            var guestIds = gamesIds.Select(x => x.GuestTeamId);
            var homeIds = gamesIds.Select(x => x.HomeTeamId);
            var teamIds = guestIds.Concat(homeIds);
            var teams = _teamsRepository.GetRange(teamIds).ToDictionary(x => x.EntityId, y => y);

            return events.Select(x => _eventMapper.Map(x, teams)); 
        }

        public void Create(string leagueId, EventVewModel model)
        {
            _eventsRepository.Create(leagueId, model);
        }

        public void Update(string leagueId, EventVewModel model)
        {
            _eventsRepository.Update(leagueId, model);
        }

        public void Delete(string eventId)
        {
            _eventsRepository.Delete(eventId);
        }
    }
}

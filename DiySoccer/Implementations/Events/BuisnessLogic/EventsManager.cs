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
        private readonly IEventsRepository _eventsRepository;
        private readonly EventsMapper _eventMapper;

        public EventsManager(IEventsRepository eventsRepository, EventsMapper eventMapper)
        {
            _eventsRepository = eventsRepository;
            _eventMapper = eventMapper;
        }

        public IEnumerable<EventVewModel> GetRange(string leagueId)
        {
            var events = _eventsRepository.GetByLeague(leagueId);
            return events.Select(x => _eventMapper.Map(x)); 
        }

        public EventVewModel Create(string leagueId)
        {
            return _eventMapper.Map(_eventsRepository.Create(leagueId));
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

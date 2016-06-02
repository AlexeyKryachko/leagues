using System.Collections.Generic;
using System.Linq;
using Interfaces.Events.BuisnessLogic;
using Interfaces.Events.BuisnessLogic.Models;
using Interfaces.Events.DataAccess;
using Interfaces.Events.DataAccess.Model;
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

        public EventGameVewModel CreateEventGame(string leagueId, string eventId)
        {
            var eventEntity = _eventsRepository.Get(eventId);

            var eventItem = new EventGameDb();
            eventItem.Id = eventEntity.Games.Any() ? eventEntity.Games.Max(x => x.Id) + 1 : 0;
            eventEntity.Games.Add(eventItem);

            _eventsRepository.Update(eventEntity);

            return _eventMapper.Map(eventItem);
        }
        
        public EventVewModel Update(string leagueId, EventVewModel model)
        {
            _eventsRepository.Update(leagueId, model);
            return model;
        }

        public void Delete(string eventId)
        {
            _eventsRepository.Delete(eventId);
        }

        public void DeleteEventGame(string eventId, int eventGameId)
        {
            var eventEntity = _eventsRepository.Get(eventId);

            var eventGame = eventEntity.Games.FirstOrDefault(x => x.Id == eventGameId);
            if (eventGame == null)
                return;

            eventEntity.Games.Remove(eventGame);
            _eventsRepository.Update(eventEntity);
        }
    }
}

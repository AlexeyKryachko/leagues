using System.Collections.Generic;
using System.Linq;
using Interfaces.Events.BuisnessLogic;
using Interfaces.Events.BuisnessLogic.Models;
using Interfaces.Events.DataAccess;
using Interfaces.Events.DataAccess.Model;
using Interfaces.Games.DataAccess;

namespace Implementations.Events.BuisnessLogic
{
    public class EventsManager : IEventsManager
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IGamesRepository _gamesRepository;
        private readonly EventsMapper _eventMapper;

        public EventsManager(
            IEventsRepository eventsRepository,
            IGamesRepository gamesRepository,
            EventsMapper eventMapper)
        {
            _eventsRepository = eventsRepository;
            _gamesRepository = gamesRepository;
            _eventMapper = eventMapper;
        }

        public IEnumerable<EventVewModel> GetRange(string leagueId)
        {
            var events = _eventsRepository.GetByLeague(leagueId);
            var games = _gamesRepository.GetByLeague(leagueId).ToList();

            return events.Select(x => _eventMapper.Map(x, games)); 
        }

        public EventVewModel Create(string leagueId)
        {
            var created = _eventsRepository.Create(leagueId);
            var games = _gamesRepository.GetByLeague(leagueId).ToList();

            return _eventMapper.Map(created, games);
        }

        public EventGameVewModel CreateEventGame(string leagueId, string eventId)
        {
            var eventEntity = _eventsRepository.Get(eventId);

            var eventItem = new EventGameDb();
            eventItem.Id = eventEntity.Games.Any() ? eventEntity.Games.Max(x => x.Id) + 1 : 0;

            var eventGames = eventEntity.Games.ToList();
            eventGames.Add(eventItem);
            eventEntity.Games = eventGames;

            _eventsRepository.Update(eventEntity);

            var games = _gamesRepository.GetByLeague(leagueId).ToList();
            return _eventMapper.Map(eventItem, games);
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

            var games = eventEntity.Games.ToList();
            games.Remove(eventGame);
            eventEntity.Games = games;

            _eventsRepository.Update(eventEntity);
        }
    }
}

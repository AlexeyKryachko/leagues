using System.Collections.Generic;
using System.Linq;
using Implementations.Core.DataAccess;
using Interfaces.Events.BuisnessLogic.Models;
using Interfaces.Events.DataAccess;
using Interfaces.Events.DataAccess.Model;
using MongoDB.Driver;

namespace Implementations.Events.DataAccess
{
    public class EventsRepository : BaseRepository<EventDb>, IEventsRepository
    {
        private readonly EventsMapper _eventMapper;

        public EventsRepository(EventsMapper eventMapper)
        {
            _eventMapper = eventMapper;
        }

        protected override string CollectionName => "events";
        
        public IEnumerable<EventDb> GetByLeague(string leagueId)
        {
            return Collection.AsQueryable().Where(x => x.LeagueId == leagueId).ToList();
        }
        
        public void Update(string leagueId, EventVewModel model)
        {
            var filter = Builders<EventDb>.Filter.Eq(x => x.LeagueId, leagueId) & Builders<EventDb>.Filter.Eq(x => x.EntityId, model.Id);

            var games = model.Games.Select(_eventMapper.Map);

            var update = Builders<EventDb>.Update
                .Set(x => x.Name, model.Name)
                .Set(x => x.StartDate, model.StartDate)
                .Set(x => x.EndDate, model.EndDate)
                .Set(x => x.Minor, model.Minor)
                .Set(x => x.Group, model.Group)
                .Set(x => x.Games, games);

            Collection.UpdateOne(filter, update);
        }

        public void Update(EventDb entity)
        {
            var filter = Builders<EventDb>.Filter.Eq(x => x.LeagueId, entity.LeagueId) & Builders<EventDb>.Filter.Eq(x => x.EntityId, entity.EntityId);
            
            var update = Builders<EventDb>.Update
                .Set(x => x.Name, entity.Name)
                .Set(x => x.StartDate, entity.StartDate)
                .Set(x => x.EndDate, entity.EndDate)
                .Set(x => x.Minor, entity.Minor)
                .Set(x => x.Games, entity.Games);

            Collection.UpdateOne(filter, update);
        }
    }
}

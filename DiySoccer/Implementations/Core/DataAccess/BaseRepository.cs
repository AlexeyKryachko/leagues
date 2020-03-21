using System.Collections.Generic;
using System.Linq;
using Interfaces.Core.DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Implementations.Core.DataAccess
{
    public abstract class BaseRepository<T> : MongoRepository<T>, IBaseRepository<T> where T: IBaseEntity, new()
    {
        public T Create(string leagueId)
        {
            var entity = new T();
            Add(leagueId, entity);
            return entity;
        }

        public void Add(string leagueId, T entity)
        {
            if (entity == null)
                return;

            entity.EntityId = ObjectId.GenerateNewId().ToString();
            entity.LeagueId = leagueId;
            Collection.InsertOne(entity);
        }

        public void AddRange(string leagueId, IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                return;

            foreach (var entity in entities)
            {
                entity.EntityId = ObjectId.GenerateNewId().ToString();
                entity.LeagueId = leagueId;
            }
            Collection.InsertMany(entities);
        }

        public T Get(string id)
        {
            return Collection.AsQueryable().FirstOrDefault(x => x.EntityId == id);
        }
        
        public IEnumerable<T> GetRange(IEnumerable<string> ids)
        {
            return Collection.AsQueryable().Where(x => ids.Contains(x.EntityId));
        }

        public IEnumerable<T> GetByLeagueId(string leagueId)
        {
            return Collection.AsQueryable().Where(x => x.LeagueId == leagueId);
        }

        public void Delete(string id)
        {
            Collection.DeleteOne(x => x.EntityId == id);
        }

        public void DeleteByLeagueId(string leagueId)
        {
            var filter = Builders<T>.Filter.Eq(x => x.LeagueId, leagueId);

            Collection.DeleteMany(filter);
        }
    }
}

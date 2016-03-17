using System.Collections.Generic;
using System.Linq;
using Interfaces.Core.DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Implementations.Core.DataAccess
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T: IBaseEntity
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        protected abstract string CollectionName { get; }
        protected IMongoCollection<T> Collection { get; }

        protected BaseRepository()
        {
            _client = new MongoClient("mongodb://7diysoccerLehaAdmin:0o9i8u@gmresearchdev.cloudapp.net:27017");
            _database = _client.GetDatabase("diysoccer");
            Collection = _database.GetCollection<T>(CollectionName);
        }
        
        public void Add(string leagueId, T entity)
        {
            entity.EntityId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            entity.LeagueId = leagueId;
            Collection.InsertOne(entity);
        }

        public void AddRange(string leagueId, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.EntityId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
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

        public void Delete(string id)
        {
            Collection.DeleteOne(x => x.EntityId == id);
        }
    }
}

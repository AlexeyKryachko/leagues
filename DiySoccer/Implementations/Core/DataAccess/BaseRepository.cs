using System.Collections.Generic;
using System.Linq;
using Interfaces.Core.DataAccess;
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
            _client = new MongoClient();
            _database = _client.GetDatabase("diysoccer");
            Collection = _database.GetCollection<T>(CollectionName);
        }
        
        public void Add(T entity)
        {
            entity.EntityId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            Collection.InsertOne(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.EntityId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
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
    }
}

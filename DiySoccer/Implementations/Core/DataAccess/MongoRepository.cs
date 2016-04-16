using System.Collections.Generic;
using System.Linq;
using Core;
using Interfaces.Core.DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Implementations.Core.DataAccess
{
    public abstract class MongoRepository<T>
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        protected abstract string CollectionName { get; }
        protected IMongoCollection<T> Collection { get; }

        protected MongoRepository()
        {
            _client = new MongoClient(MongoConnetcionString.ConnectionString);
            _database = _client.GetDatabase(MongoConnetcionString.Database);
            Collection = _database.GetCollection<T>(CollectionName);
        }
    }
}

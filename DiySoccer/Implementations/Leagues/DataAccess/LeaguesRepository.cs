using System.Collections.Generic;
using Interfaces.Leagues.DataAccess;
using Interfaces.Leagues.DataAccess.Model;
using MongoDB.Driver;
using System.Linq;
using Core;
using MongoDB.Bson;

namespace Implementations.Leagues.DataAccess
{
    public sealed class LeaguesRepository : ILeaguesRepository
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        private const string CollectionName = "leagues";
        private IMongoCollection<LeagueDb> Collection { get; }

        public LeaguesRepository()
        {
            _client = new MongoClient(MongoConnetcionString.ConnectionString);
            _database = _client.GetDatabase(MongoConnetcionString.Database);
            Collection = _database.GetCollection<LeagueDb>(CollectionName);
        }

        public IEnumerable<LeagueDb> GetAll()
        {
            return Collection.AsQueryable().ToList();
        }

        public LeagueDb Get(string leagueId)
        {
            return Collection.AsQueryable().FirstOrDefault(x => x.EntityId == leagueId);
        }

        public void Create(string name, string description)
        {
            var entity = new LeagueDb
            {
                EntityId = ObjectId.GenerateNewId().ToString(),
                Name = name,
                Description = description
            };

            Collection.InsertOne(entity);
        }

        public void Update(string leagueId, string name, string description)
        {
            var filter = Builders<LeagueDb>.Filter.Eq(x => x.EntityId, leagueId);
            var update = Builders<LeagueDb>.Update
                .Set(x => x.Name, name)
                .Set(x => x.Description, description);

            Collection.UpdateOne(filter, update);
        }
    }
}

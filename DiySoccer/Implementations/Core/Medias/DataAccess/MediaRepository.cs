using System.Collections.Generic;
using Core;
using Interfaces.Core.Services.Medias.DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using MongoDB.Driver.Linq;

namespace Implementations.Core.Medias.DataAccess
{
    public class MediaRepository : IMediaRepository
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        private const string CollectionName = "medias";
        private IMongoCollection<MediaDb> Collection { get; }

        public MediaRepository()
        {
            _client = new MongoClient(MongoConnetcionString.ConnectionString);
            _database = _client.GetDatabase(MongoConnetcionString.Database);
            Collection = _database.GetCollection<MediaDb>(CollectionName);
        }

        public MediaDb Get(string mediaId)
        {
            return Collection.AsQueryable().FirstOrDefault(x => x.EntityId == mediaId);
        }

        public IEnumerable<MediaDb> GetRange(IEnumerable<string> mediaIds)
        {
            if (mediaIds == null || !mediaIds.Any())
                return Enumerable.Empty<MediaDb>();

            return Collection.AsQueryable().Where(x => mediaIds.Contains(x.EntityId)).ToList();
        }

        public MediaDb Add(MediaDb entity)
        {
            entity.EntityId = ObjectId.GenerateNewId().ToString();

            Collection.InsertOne(entity);
            return entity;
        }
    }
}

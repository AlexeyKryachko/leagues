using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Leagues.DataAccess.Model
{
    public class LeagueDb
    {
        [BsonId]
        public string EntityId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("desc")]
        public string Description { get; set; }
    }
}

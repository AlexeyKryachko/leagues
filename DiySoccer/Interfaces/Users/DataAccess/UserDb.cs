using Interfaces.Core.DataAccess;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Users.DataAccess
{
    public class UserDb : IBaseEntity
    {
        [BsonId]
        public string EntityId { get; set; }

        [BsonElement("lid")]
        public string LeagueId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}

using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Leagues.DataAccess.Model
{
    [BsonIgnoreExtraElements]
    public class LeagueDb
    {
        [BsonId]
        public string EntityId { get; set; }

        [BsonElement("type")]
        public LeagueType Type { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("subName")]
        public string SubName { get; set; }
        
        [BsonElement("desc")]
        public string Description { get; set; }

        [BsonElement("inf")]
        public string Information { get; set; }

        [BsonElement("vkgroup")]
        public string VkSecurityGroup { get; set; }

        [BsonElement("admins")]
        public IEnumerable<string> Admins { get; set; }

        [BsonElement("media")]
        public string MediaId { get; set; }

        public LeagueDb()
        {
            Admins = Enumerable.Empty<string>();
        }
    }
}

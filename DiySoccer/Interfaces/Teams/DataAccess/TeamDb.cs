using System.Collections.Generic;
using Interfaces.Core.DataAccess;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Teams.DataAccess
{
    public class TeamDb : IBaseEntity
    {
        [BsonId]
        public string EntityId { get; set; }
        
        [BsonElement("lid")]
        public string LeagueId { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("hidden")]
        public bool Hidden { get; set; }

        [BsonElement("members")]
        public IEnumerable<string> MemberIds { get; set; }
    }
}

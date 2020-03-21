using Interfaces.Core.DataAccess;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.GameApproval.DataAccess.Model
{
    public class GameApprovalDb : IBaseEntity
    {
        [BsonId]
        public string EntityId { get; set; }

        [BsonElement("leagueId")]
        public string LeagueId { get; set; }

        [BsonElement("eId")]
        public string EventId { get; set; }
        
        [BsonElement("homeTeam")]
        public TeamGameApprovalDb HomeTeam { get; set; }

        [BsonElement("guestTeam")]
        public TeamGameApprovalDb GuestTeam { get; set; }
    }
}

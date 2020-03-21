using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.GameApproval.DataAccess.Model
{
    public class TeamGameApprovalDb
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("score")]
        public int Score { get; set; }

        [BsonElement("bestId")]
        public string BestMemberId { get; set; }

        [BsonElement("members")]
        public IEnumerable<MemberGameApprovalDb> Members { get; set; }

        public TeamGameApprovalDb()
        {
            Members = Enumerable.Empty<MemberGameApprovalDb>();
        }
    }
}

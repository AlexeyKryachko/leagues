using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.GameApproval.DataAccess.Model
{
    public class MemberGameApprovalDb
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("score")]
        public int Score { get; set; }

        [BsonElement("help")]
        public int Help { get; set; }
    }
}

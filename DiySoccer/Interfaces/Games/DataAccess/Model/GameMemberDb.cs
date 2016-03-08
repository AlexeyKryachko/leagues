using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Games.DataAccess.Model
{
    public class GameMemberDb
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("score")]
        public int Score { get; set; }

        [BsonElement("help")]
        public int Help { get; set; }
    }
}

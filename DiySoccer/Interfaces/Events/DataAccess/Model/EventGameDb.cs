using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Events.DataAccess.Model
{
    public class EventGameDb
    {
        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("homeTeamId")]
        public string HomeTeamId { get; set; }

        [BsonElement("guestTeamId")]
        public string GuestTeamId { get; set; }
    }
}

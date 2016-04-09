using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Core.Services.Medias.DataAccess
{
    public class MediaDb
    {
        [BsonId]
        public string EntityId { get; set; }

        [BsonElement("url")]
        public string RelativeUrl { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("type")]
        public string ContentType { get; set; }
    }
}

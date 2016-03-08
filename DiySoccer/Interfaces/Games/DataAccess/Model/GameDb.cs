using Interfaces.Core.DataAccess;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Games.DataAccess.Model
{
    public class GameDb : IBaseEntity
    {
        [BsonId]
        public string EntityId { get; set; }

        [BsonElement("leagueId")]
        public string LeagueId { get; set; }

        [BsonElement("homeTeam")]
        public GameTeamDb HomeTeam { get; set; }

        [BsonElement("guestTeam")]
        public GameTeamDb GuestTeam { get; set; }
    }
}

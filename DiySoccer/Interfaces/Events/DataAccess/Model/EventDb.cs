using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Core.DataAccess;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Events.DataAccess.Model
{
    [BsonIgnoreExtraElements]
    public class EventDb : IBaseEntity
    {
        [BsonId]
        public string EntityId { get; set; }

        [BsonElement("leagueId")]
        public string LeagueId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("startDate")]
        public DateTime? StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime? EndDate { get; set; }

        [BsonElement("games")]
        public IList<EventGameDb> Games { get; set; }

        public EventDb()
        {
            Games = new List<EventGameDb>();
        }
    }
}

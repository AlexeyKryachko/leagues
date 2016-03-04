﻿using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Games.DataAccess.Model
{
    public class GameTeamDb
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("members")]
        public IEnumerable<GameMemberDb> Members { get; set; }

        public GameTeamDb()
        {
            Members = Enumerable.Empty<GameMemberDb>();
        }
    }
}

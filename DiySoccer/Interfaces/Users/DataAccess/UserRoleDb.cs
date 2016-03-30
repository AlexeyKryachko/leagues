using Microsoft.AspNet.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Users.DataAccess
{
    public class UserRoleDb : IRole
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}

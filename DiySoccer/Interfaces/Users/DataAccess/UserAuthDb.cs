using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Users.DataAccess
{
    public class UserAuthDb : IUser
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("username")]
        public string UserName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("isAdmin")]
        public bool IsAdmin { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserAuthDb> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}

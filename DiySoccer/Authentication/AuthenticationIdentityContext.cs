using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Interfaces.Users.DataAccess;
using MongoDB.Driver;

namespace Authentication
{
    public class AuthenticationIdentityContext : IDisposable
    {
        public static AuthenticationIdentityContext Create()
        {
            // todo add settings where appropriate to switch server & database in your own application
            var client = new MongoClient(MongoConnetcionString.ConnectionString);
            var database = client.GetDatabase(MongoConnetcionString.Database);
            var users = database.GetCollection<UserAuthDb>("users");
            var roles = database.GetCollection<UserRoleDb>("roles");
            return new AuthenticationIdentityContext(users, roles);
        }

        private AuthenticationIdentityContext(IMongoCollection<UserAuthDb> users, IMongoCollection<UserRoleDb> roles)
        {
            Users = users;
            Roles = roles;
        }

        public IMongoCollection<UserRoleDb> Roles { get; set; }

        public IMongoCollection<UserAuthDb> Users { get; set; }

        public Task<List<UserRoleDb>> AllRolesAsync()
        {
            return Roles.Find(r => true).ToListAsync();
        }

        public void Dispose()
        {
        }
    }
}
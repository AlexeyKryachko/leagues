using System.Threading.Tasks;
using Interfaces.Users.DataAccess;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;

namespace Authentication.Mongo
{
    public class MongoRoleStore : IRoleStore<UserRoleDb>
    {
        private readonly IMongoCollection<UserRoleDb> _mongoCollection;

        public MongoRoleStore(IMongoCollection<UserRoleDb> mongoCollection)
        {
            _mongoCollection = mongoCollection;
        }

        public void Dispose()
        {
        }

        public Task CreateAsync(UserRoleDb role)
        {
            return _mongoCollection.InsertOneAsync(role);
        }

        public Task UpdateAsync(UserRoleDb role)
        {
            var filter = Builders<UserRoleDb>.Filter.Eq(x => x.Id, role.Id);
            var update = Builders<UserRoleDb>.Update
                .Set(x => x.Name, role.Name);

            return _mongoCollection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(UserRoleDb role)
        {
            var filter = Builders<UserRoleDb>.Filter.Eq(x => x.Id, role.Id);

            return _mongoCollection.DeleteOneAsync(filter);
        }

        public Task<UserRoleDb> FindByIdAsync(string roleId)
        {
            var filter = Builders<UserRoleDb>.Filter.Eq(x => x.Id, roleId);
            
            return _mongoCollection.Find(filter).SingleAsync();
        }

        public Task<UserRoleDb> FindByNameAsync(string roleName)
        {
            var filter = Builders<UserRoleDb>.Filter.Eq(x => x.Name, roleName);

            return _mongoCollection.Find(filter).SingleAsync();
        }
    }
}

using System;
using System.Threading.Tasks;
using Interfaces.Users.DataAccess;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;

namespace Authentication.Mongo
{
    public class MongoUserStore : IUserStore<UserAuthDb>
    {
        private readonly IMongoCollection<UserAuthDb> _mongoCollection;

        public MongoUserStore(IMongoCollection<UserAuthDb> mongoCollection)
        {
            _mongoCollection = mongoCollection;
        }

        public void Dispose()
        {
        }

        public Task CreateAsync(UserAuthDb user)
        {
            return _mongoCollection.InsertOneAsync(user);
        }

        public Task UpdateAsync(UserAuthDb user)
        {
            var filter = Builders<UserAuthDb>.Filter.Eq(x => x.Id, user.Id);
            var update = Builders<UserAuthDb>.Update
                .Set(x => x.UserName, user.UserName)
                .Set(x => x.Email, user.Email);

            return _mongoCollection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(UserAuthDb user)
        {
            var filter = Builders<UserAuthDb>.Filter.Eq(x => x.Id, user.Id);

            return _mongoCollection.DeleteOneAsync(filter);
        }

        public Task<UserAuthDb> FindByIdAsync(string userId)
        {
            var filter = Builders<UserAuthDb>.Filter.Eq(x => x.Id, userId);

            return _mongoCollection.Find(filter).SingleAsync();
        }

        public Task<UserAuthDb> FindByNameAsync(string userName)
        {
            var filter = Builders<UserAuthDb>.Filter.Eq(x => x.UserName, userName);

            return _mongoCollection.Find(filter).SingleAsync();
        }
    }
}

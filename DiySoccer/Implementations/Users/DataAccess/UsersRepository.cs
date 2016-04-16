using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Implementations.Core.DataAccess;
using Interfaces.Users.DataAccess;
using MongoDB.Driver;

namespace Implementations.Users.DataAccess
{
    public class UsersRepository : MongoRepository<UserAuthDb>, IUsersRepository
    {
        protected override string CollectionName => "authUsers";
        
        public void Dispose()
        {
        }

        public Task CreateAsync(UserAuthDb user)
        {
            return Collection.InsertOneAsync(user);
        }

        public Task UpdateAsync(UserAuthDb user)
        {
            var filter = Builders<UserAuthDb>.Filter.Eq(x => x.Id, user.Id);
            var update = Builders<UserAuthDb>.Update
                .Set(x => x.UserName, user.UserName)
                .Set(x => x.Email, user.Email);

            return Collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(UserAuthDb user)
        {
            var filter = Builders<UserAuthDb>.Filter.Eq(x => x.Id, user.Id);

            return Collection.DeleteOneAsync(filter);
        }

        public Task<UserAuthDb> FindByIdAsync(string userId)
        {
            var filter = Builders<UserAuthDb>.Filter.Eq(x => x.Id, userId);

            return Collection.Find(filter).SingleAsync();
        }

        public Task<UserAuthDb> FindByNameAsync(string userName)
        {
            var filter = Builders<UserAuthDb>.Filter.Eq(x => x.UserName, userName);

            return Collection.Find(filter).SingleAsync();
        }

        public IEnumerable<UserAuthDb> GetRange(IEnumerable<string> ids)
        {
            return Collection.AsQueryable().Where(x => ids.Contains(x.Id));
        }

        public UserAuthDb GetByUserName(string userName)
        {
            return Collection.AsQueryable().FirstOrDefault(x => x.UserName == userName);
        }

        public IEnumerable<UserAuthDb> Find(string query, int page, int pageSize)
        {
            return Collection.AsQueryable().Where(x => x.UserName.Contains(query) || x.Email.Contains(query));
        }
    }
}

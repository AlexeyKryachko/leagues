using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces.Users.DataAccess;
using Microsoft.AspNet.Identity;

namespace Authentication.Mongo
{
    public class MongoUserLoginStore : IUserLoginStore<UserAuthDb>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(UserAuthDb user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserAuthDb user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(UserAuthDb user)
        {
            throw new NotImplementedException();
        }

        public Task<UserAuthDb> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserAuthDb> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task AddLoginAsync(UserAuthDb user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(UserAuthDb user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(UserAuthDb user)
        {
            throw new NotImplementedException();
        }

        public Task<UserAuthDb> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Interfaces.Users.DataAccess
{
    public interface IUsersRepository : IUserStore<UserAuthDb>
    {
        IEnumerable<UserAuthDb> GetRange(IEnumerable<string> ids);

        UserAuthDb GetByUserName(string userName);

        IEnumerable<UserAuthDb> Find(string query, int page, int pageSize);
    }
}

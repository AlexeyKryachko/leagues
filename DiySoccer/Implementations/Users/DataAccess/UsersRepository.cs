using Implementations.Core.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Users.DataAccess
{
    public class UsersRepository : BaseRepository<UserDb>, IUsersRepository
    {
        protected override string CollectionName => "users";
    }
}

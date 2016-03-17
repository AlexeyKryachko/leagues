using System.Collections.Generic;
using Implementations.Core.DataAccess;
using Interfaces.Users.DataAccess;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Implementations.Users.DataAccess
{
    public class UsersRepository : BaseRepository<UserDb>, IUsersRepository
    {
        protected override string CollectionName => "users";

        public IEnumerable<UserDb> Find(string query)
        {
            return Collection.AsQueryable().Where(x => x.Name.Contains(query));
        }

        public IEnumerable<UserDb> Find(string leagueId, string query, int page, int pageSize)
        {
            return Collection.AsQueryable()
                .Where(x => x.LeagueId == leagueId && 
                    x.Name.Contains(query))
                .Skip(page * pageSize)
                .Take(pageSize);
        }
    }
}

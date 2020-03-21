using System.Collections.Generic;
using Implementations.Core.DataAccess;
using Interfaces.Users.DataAccess;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Implementations.Users.DataAccess
{
    public class PlayersRepository : BaseRepository<UserDb>, IPlayersRepository
    {
        protected override string CollectionName => "users";

        public IEnumerable<UserDb> Find(string query)
        {
            return Collection.AsQueryable().Where(x => x.Name.Contains(query));
        }

        public IEnumerable<UserDb> Find(string leagueId, string query, int page, int pageSize)
        {
            var queryable = Collection.AsQueryable();

            if (!string.IsNullOrEmpty(leagueId))
            {
                queryable = queryable.Where(x => x.LeagueId == leagueId);
            }

            if (!string.IsNullOrEmpty(query))
            {
                queryable = queryable.Where(x => x.Name.Contains(query));
            }

            return queryable
                .Skip(page * pageSize)
                .Take(pageSize);
        }
    }
}

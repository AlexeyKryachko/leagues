using System.Collections.Generic;
using System.Linq;
using Implementations.Core.DataAccess;
using Interfaces.Teams.DataAccess;
using MongoDB.Driver;

namespace Implementations.Teams.DataAccess
{
    public class TeamsRepository : BaseRepository<TeamDb>, ITeamsRepository
    {
        protected override string CollectionName => "teams";

        public void Create(string leagueId, string name, bool hidden, IEnumerable<string> memberIds)
        {
            var entity = new TeamDb
            {
                Name = name,
                Hidden = hidden,
                MemberIds = memberIds
            };

            Add(leagueId, entity);
        }

        public IEnumerable<TeamDb> GetByLeague(string id)
        {
            return Collection.AsQueryable().Where(x => x.LeagueId == id);
        }

        public void Update(string leagueId, string id, string name, bool hidden, IEnumerable<string> memberIds)
        {
            var filter = Builders<TeamDb>.Filter.Eq(x => x.LeagueId, leagueId) & Builders<TeamDb>.Filter.Eq(x => x.EntityId, id);
            var update = Builders<TeamDb>.Update
                .Set(x => x.Name, name)
                .Set(x => x.Hidden, hidden)
                .Set(x => x.MemberIds, memberIds);

            Collection.UpdateOne(filter, update);
        }

        public IEnumerable<TeamDb> FindByUsers(string leagueId, IEnumerable<string> userIds)
        {
            return Collection.AsQueryable().Where(x => x.LeagueId == leagueId && x.MemberIds.Any(userId => userIds.Contains(userId)));
        }
    }
}

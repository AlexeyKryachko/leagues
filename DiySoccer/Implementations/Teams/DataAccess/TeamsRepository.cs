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
        
        public IEnumerable<TeamDb> GetByLeague(string id)
        {
            return Collection.AsQueryable().Where(x => x.LeagueId == id);
        }

        public void Update(string leagueId, TeamDb entity)
        {
            var filter = Builders<TeamDb>.Filter.Eq(x => x.LeagueId, leagueId) & Builders<TeamDb>.Filter.Eq(x => x.EntityId, entity.EntityId);
            var update = Builders<TeamDb>.Update
                .Set(x => x.Name, entity.Name)
                .Set(x => x.Hidden, entity.Hidden)
                .Set(x => x.MemberIds, entity.MemberIds)
                .Set(x => x.MediaId, entity.MediaId);

            Collection.UpdateOne(filter, update);
        }

        public IEnumerable<TeamDb> FindByUsers(string leagueId, IEnumerable<string> userIds)
        {
            return Collection.AsQueryable().Where(x => x.LeagueId == leagueId && x.MemberIds.Any(userId => userIds.Contains(userId)));
        }
    }
}

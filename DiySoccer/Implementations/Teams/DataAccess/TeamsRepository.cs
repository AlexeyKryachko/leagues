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

        public void Create(string leagueId, string name, IEnumerable<string> memberIds)
        {
            var entity = new TeamDb
            {
                LeagueId = leagueId,
                Name = name,
                MemberIds = memberIds
            };

            Add(entity);
        }

        public IEnumerable<TeamDb> GetByLeague(string id)
        {
            return Collection.AsQueryable().Where(x => x.LeagueId == id);
        }
    }
}

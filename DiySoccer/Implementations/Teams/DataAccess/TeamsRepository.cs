using System.Collections.Generic;
using Implementations.Core.DataAccess;
using Interfaces.Teams.DataAccess;

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
    }
}

using System.Collections.Generic;
using Interfaces.Core.DataAccess;

namespace Interfaces.Teams.DataAccess
{
    public interface ITeamsRepository : IBaseRepository<TeamDb>
    {
        IEnumerable<TeamDb> GetByLeague(string id);

        void Update(string leagueId, TeamDb entity);

        IEnumerable<TeamDb> FindByUsers(string leagueId, IEnumerable<string> userIds);
    }
}

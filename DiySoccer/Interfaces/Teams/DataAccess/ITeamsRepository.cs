using System.Collections.Generic;
using Interfaces.Core.DataAccess;

namespace Interfaces.Teams.DataAccess
{
    public interface ITeamsRepository : IBaseRepository<TeamDb>
    {
        IEnumerable<TeamDb> GetByLeague(string id);

        IEnumerable<TeamDb> FindByUsers(string leagueId, IEnumerable<string> userIds);
        IEnumerable<TeamDb> Find(string query, int page, int pageSize);

        void Update(string leagueId, TeamDb entity);
    }
}

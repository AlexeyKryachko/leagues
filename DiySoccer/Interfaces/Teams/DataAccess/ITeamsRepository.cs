using System.Collections.Generic;
using Interfaces.Core.DataAccess;

namespace Interfaces.Teams.DataAccess
{
    public interface ITeamsRepository : IBaseRepository<TeamDb>
    {
        void Create(string leagueId, string name, IEnumerable<string> memberIds);

        IEnumerable<TeamDb> GetByLeague(string id);

        void Update(string leagueId, string id, string name, IEnumerable<string> memberIds);
    }
}

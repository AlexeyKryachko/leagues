using System.Collections.Generic;
using Interfaces.Core.DataAccess;

namespace Interfaces.Teams.DataAccess
{
    public interface ITeamsRepository : IBaseRepository<TeamDb>
    {
        void Create(string leagueId, string name, IEnumerable<string> memberIds);
    }
}

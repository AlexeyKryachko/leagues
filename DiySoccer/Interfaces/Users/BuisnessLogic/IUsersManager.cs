using System.Collections.Generic;
using Interfaces.Core;

namespace Interfaces.Users.BuisnessLogic
{
    public interface IUsersManager
    {
        IEnumerable<IdNameViewModel> Find(string leagueId, string query, IEnumerable<string> excludeTeamIds, int page, int pageSize);
    }
}

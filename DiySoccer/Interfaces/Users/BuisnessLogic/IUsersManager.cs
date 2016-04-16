using System.Collections.Generic;
using Interfaces.Core;

namespace Interfaces.Users.BuisnessLogic
{
    public interface IUsersManager
    {
        IEnumerable<IdNameViewModel> FindPlayer(string leagueId, string query, IEnumerable<string> excludeTeamIds, int page, int pageSize);

        IEnumerable<IdNameViewModel> FindUser(string query, int page, int pageSize);
    }
}

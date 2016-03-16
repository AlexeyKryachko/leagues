using System.Collections.Generic;
using Interfaces.Users.DataAccess;

namespace Interfaces.Users.BuisnessLogic
{
    public interface IUsersManager
    {
        IEnumerable<UserDb> Find(string leagueId, string query, string excludeTeamId, int page, int pageSize);
    }
}

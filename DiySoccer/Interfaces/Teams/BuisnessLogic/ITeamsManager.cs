using System.Collections.Generic;
using Interfaces.Core;
using Interfaces.Teams.BuisnessLogic.Models;

namespace Interfaces.Teams.BuisnessLogic
{
    public interface ITeamsManager
    {
        TeamViewModel Get(string leagueId, string teamId);
        TeamInfoViewModel GetInfo(string leagueId, string teamId);
        IEnumerable<TeamViewModel> GetByLeague(string id);

        IEnumerable<IdNameViewModel> Find(string query, int page, int pageSize);

        void Create(string leagueId, TeamViewModel model);
        TeamViewModel Copy(string teamId, string destinationUnionId);
        void Update(string leagueId, string teamId, TeamViewModel model);
    }
}

using System.Collections.Generic;
using Interfaces.Teams.BuisnessLogic.Models;

namespace Interfaces.Teams.BuisnessLogic
{
    public interface ITeamsManager
    {
        void Create(string leagueId, TeamViewModel model);

        TeamViewModel Get(string leagueId, string teamId);

        TeamInfoViewModel GetInfo(string leagueId, string teamId);

        void Update(string leagueId, string teamId, TeamViewModel model);

        IEnumerable<TeamViewModel> GetByLeague(string id);
    }
}

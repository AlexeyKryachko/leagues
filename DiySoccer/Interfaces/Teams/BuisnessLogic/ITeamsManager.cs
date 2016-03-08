using System.Collections.Generic;
using Interfaces.Teams.BuisnessLogic.Models;

namespace Interfaces.Teams.BuisnessLogic
{
    public interface ITeamsManager
    {
        void Create(CreateTeamViewModel model);

        IEnumerable<TeamViewModel> GetByLeague(string id);

        IEnumerable<TeamStatisticViewModel> GetStatisticByLeague(string leagueId);
    }
}

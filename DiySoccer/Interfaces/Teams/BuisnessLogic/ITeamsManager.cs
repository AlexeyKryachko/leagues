using System.Collections.Generic;
using Interfaces.Teams.BuisnessLogic.Models;

namespace Interfaces.Teams.BuisnessLogic
{
    public interface ITeamsManager
    {
        void Create(string leagueId, CreateTeamViewModel model);

        TeamViewModel Get(string leagueId, string teamId);

        TeamInfoViewModel GetInfo(string leagueId, string teamId);

        void Update(string leagueId, string teamId, CreateTeamViewModel model);

        IEnumerable<TeamViewModel> GetByLeague(string id);

        IEnumerable<TeamStatisticViewModel> GetStatisticByLeague(string leagueId);
    }
}

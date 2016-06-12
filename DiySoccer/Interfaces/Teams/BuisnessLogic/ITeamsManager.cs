using System.Collections.Generic;
using Interfaces.Games.DataAccess.Model;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Teams.BuisnessLogic.Models;
using Interfaces.Teams.DataAccess;

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

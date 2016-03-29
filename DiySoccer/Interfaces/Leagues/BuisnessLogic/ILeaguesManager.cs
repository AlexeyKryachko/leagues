using System.Collections.Generic;
using Interfaces.Leagues.BuisnessLogic.Model;

namespace Interfaces.Leagues.BuisnessLogic
{
    public interface ILeaguesManager
    {
        LeagueStatisticViewModel GetStatisticByLeague(string leagueId);

        IEnumerable<LeagueViewModel> GetAll();

        LeagueViewModel Get(string leagueId);

        void Create(LeagueViewModel model);

        void Update(LeagueViewModel model);

        void Delete(string leagueId);
    }
}

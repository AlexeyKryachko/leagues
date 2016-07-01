using System.Collections.Generic;
using Interfaces.Leagues.BuisnessLogic.Model;

namespace Interfaces.Leagues.BuisnessLogic
{
    public interface ILeaguesManager
    {
        LeagueStatisticViewModel GetStatisticByLeague(string leagueId);
        LeagueInfoViewModel GetInfo(string leagueId);
        LeaguesViewModel GetLeagues();
        IEnumerable<LeagueUnsecureViewModel> GetAllUnsecure();
        LeagueUnsecureViewModel GetUnsecure(string leagueId);

        void Create(LeagueUnsecureViewModel model);
        void Update(LeagueUnsecureViewModel model);
        void Delete(string leagueId);
    }
}

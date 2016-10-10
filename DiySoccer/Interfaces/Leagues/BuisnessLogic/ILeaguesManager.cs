using System.Collections.Generic;
using Interfaces.Leagues.BuisnessLogic.Model;

namespace Interfaces.Leagues.BuisnessLogic
{
    public interface ILeaguesManager
    {
        LeagueStatisticsViewModel GetStatisticByLeague(string leagueId);
        LeagueTableViewModel GetTable(string leagueId);
        LeagueInfoViewModel GetInfo(string leagueId);
        LeaguesViewModel GetLeagues();
        IEnumerable<LeagueUnsecureViewModel> GetAllUnsecure();
        LeagueUnsecureViewModel GetUnsecure(string leagueId);

        void Create(LeagueUnsecureViewModel model);
        void Update(LeagueUnsecureViewModel model);
        void Delete(string leagueId);
    }
}

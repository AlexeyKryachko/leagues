using System.Collections.Generic;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Leagues.DataAccess.Model;

namespace Interfaces.Leagues.DataAccess
{
    public interface ILeaguesRepository
    {
        IEnumerable<LeagueDb> GetAll();

        LeagueDb Get(string leagueId);

        void Create(LeagueUnsecureViewModel model);

        void Update(LeagueUnsecureViewModel model);

        void Delete(string leagueId);
    }
}

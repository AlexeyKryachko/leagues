using System.Collections.Generic;
using Interfaces.Leagues.DataAccess.Model;

namespace Interfaces.Leagues.DataAccess
{
    public interface ILeaguesRepository
    {
        IEnumerable<LeagueDb> GetAll();

        LeagueDb Get(string leagueId);

        void Create(string name, string description, string vkGroup);

        void Update(string leagueId, string name, string description, string vkGroup);
    }
}

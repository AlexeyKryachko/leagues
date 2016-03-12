using System.Collections.Generic;
using Interfaces.Core.DataAccess;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess.Model;

namespace Interfaces.Games.DataAccess
{
    public interface IGamesRepository : IBaseRepository<GameDb>
    {
        void Create(string leagueId, GameVewModel model);

        IEnumerable<GameDb> GetByLeague(string leagueId);

        IEnumerable<GameDb> GetByTeam(string leagueId, string teamId);
    }
}

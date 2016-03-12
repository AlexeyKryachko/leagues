using System.Collections.Generic;
using Interfaces.Core.DataAccess;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess.Model;

namespace Interfaces.Games.DataAccess
{
    public interface IGamesRepository : IBaseRepository<GameDb>
    {
        IEnumerable<GameDb> GetByLeague(string leagueId);

        IEnumerable<GameDb> GetByTeam(string leagueId, string teamId);

        void Create(string leagueId, GameVewModel model);

        void Update(string leagueId, string id, int guestScore, IEnumerable<GameMemberDb> guestMembersScores,
            int homeScore, IEnumerable<GameMemberDb> homeMembersScores);
    }
}

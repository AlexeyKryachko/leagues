using Interfaces.Games.BuisnessLogic.Models;

namespace Interfaces.Games.BuisnessLogic
{
    public interface IGamesManager
    {
        GameVewModel Get(string leagueId, string gameId);
        GameExternalViewModel GetExternal(string leagueId);

        GameInfoViewModel GetInfo(string leagueId, string gameId);

        void Create(string leagueId, GameVewModel model);

        void Update(string leagueId, string gameId, GameVewModel model);

        void Delete(string gameId);
    }
}

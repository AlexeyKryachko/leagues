using Interfaces.Games.BuisnessLogic;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess;

namespace Implementations.Games.BuisnessLogic
{
    public class GamesManager : IGamesManager
    {
        private readonly IGamesRepository _gamesRepository;

        public GamesManager(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public void Create(GameVewModel model)
        {
            _gamesRepository.Create(model);
        }
    }
}

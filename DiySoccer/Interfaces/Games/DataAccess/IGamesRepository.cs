using Interfaces.Core.DataAccess;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess.Model;

namespace Interfaces.Games.DataAccess
{
    public interface IGamesRepository : IBaseRepository<GameDb>
    {
        void Create(GameVewModel model);
    }
}

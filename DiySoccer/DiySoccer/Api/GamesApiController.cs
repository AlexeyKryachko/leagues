using System.Linq;
using System.Web.Http;
using Interfaces.Games.BuisnessLogic;
using Interfaces.Games.BuisnessLogic.Models;

namespace DiySoccer.Api
{
    public class GamesController : BaseApiController
    {
        private readonly IGamesManager _gamesManager;

        public GamesController(IGamesManager gamesManager)
        {
            _gamesManager = gamesManager;
        }

        [Route("api/games")]
        [HttpPost]
        public void Create([FromBody]GameVewModel model)
        {
            _gamesManager.Create(model);
        }
    }
}
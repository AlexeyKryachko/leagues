
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

        [Route("api/leagues/{leagueId}/games")]
        [HttpPost]
        public void Create([FromUri]string leagueId, [FromBody]GameVewModel model)
        {
            _gamesManager.Create(leagueId, model);
        }
    }
}
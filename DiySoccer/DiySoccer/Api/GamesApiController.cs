
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

        #region GET
        
        [Route("api/leagues/{leagueId}/games/{gameId}/info")]
        [HttpGet]
        public IHttpActionResult GetGame(string leagueId, string gameId)
        {
            var game = _gamesManager.Get(leagueId, gameId);
            return Json(game);
        }

        #endregion

        #region PUT

        [Route("api/leagues/{leagueId}/games/{gameId}")]
        [HttpPut]
        public void Update([FromUri]string leagueId, [FromUri]string gameId, [FromBody]GameVewModel model)
        {
            _gamesManager.Update(leagueId, gameId, model);
        }

        #endregion

        #region POST

        [Route("api/leagues/{leagueId}/games")]
        [HttpPost]
        public void Create([FromUri]string leagueId, [FromBody]GameVewModel model)
        {
            _gamesManager.Create(leagueId, model);
        }

        #endregion

        #region DELETE

        [Route("api/leagues/{leagueId}/games/{gameId}")]
        [HttpDelete]
        public void Delete(string leagueId, string gameId)
        {
            _gamesManager.Delete(gameId);
        }

        #endregion
    }
}
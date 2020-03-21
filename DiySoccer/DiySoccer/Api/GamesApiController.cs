
using System.Web.Http;
using DiySoccer.Core.Attributes;
using Interfaces.Core;
using Interfaces.GameApproval.BuisnessLogic;
using Interfaces.Games.BuisnessLogic;
using Interfaces.Games.BuisnessLogic.Models;

namespace DiySoccer.Api
{
    public class GamesController : BaseApiController
    {
        private readonly IGamesManager _gamesManager;
        private readonly IGameApprovalManager _gameApprovalManager;

        public GamesController(IGamesManager gamesManager, IGameApprovalManager gameApprovalManager)
        {
            _gamesManager = gamesManager;
            _gameApprovalManager = gameApprovalManager;
        }

        #region GET
        
        [Route("api/leagues/{leagueId}/games/{gameId}/info")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetGame(string leagueId, string gameId)
        {
            var game = _gamesManager.Get(leagueId, gameId);
            return Json(game);
        }

        [Route("api/leagues/{leagueId}/games/{gameId}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetGameInfo(string leagueId, string gameId)
        {
            var game = _gamesManager.GetInfo(leagueId, gameId);
            return Json(game);
        }

        [Route("api/leagues/{leagueId}/games/external")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetGameExternal(string leagueId)
        {
            var model = _gamesManager.GetExternal(leagueId);

            return Json(model);
        }

        #endregion

        #region PUT

        [Route("api/leagues/{leagueId}/games/{gameId}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPut]
        public void Update([FromUri]string leagueId, [FromUri]string gameId, [FromBody]GameVewModel model)
        {
            _gamesManager.Update(leagueId, gameId, model);
        }

        #endregion

        #region POST

        [Route("api/leagues/{leagueId}/games")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPost]
        public void Create([FromUri]string leagueId, [FromBody]GameVewModel model)
        {
            _gamesManager.Create(leagueId, model);
        }

        [Route("api/leagues/{leagueId}/games/external")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpPost]
        public IHttpActionResult PostGameExternal([FromUri]string leagueId, GameExternalViewModel model)
        {
            _gameApprovalManager.Create(leagueId, model);

            return Ok();
        }

        #endregion

        #region DELETE

        [Route("api/leagues/{leagueId}/games/{gameId}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpDelete]
        public void Delete(string leagueId, string gameId)
        {
            _gamesManager.Delete(gameId);
        }

        #endregion
    }
}
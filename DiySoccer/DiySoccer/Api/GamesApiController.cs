
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

        [Route("api/leagues/{leagueId}/games/approval")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpGet]
        public IHttpActionResult GetGameApproval(string leagueId)
        {
            var model = _gameApprovalManager.GetApprovals(leagueId);

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

        [Route("api/leagues/{leagueId}/games/approve/{id}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPost]
        public IHttpActionResult GetApprove(string leagueId, string id)
        {
            _gameApprovalManager.Approve(leagueId, id);

            return Ok();
        }

        [Route("api/leagues/{leagueId}/games/decline/{id}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPost]
        public IHttpActionResult GetDecline(string leagueId, string id)
        {
            _gameApprovalManager.Delete(id);

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
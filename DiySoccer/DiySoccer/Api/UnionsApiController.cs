using System.Web.Http;
using DiySoccer.Core.Attributes;
using Interfaces.Core;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Teams.BuisnessLogic;

namespace DiySoccer.Api
{
    public class UnionsApiController : BaseApiController
    {
        private readonly ITeamsManager _teamsManager;

        public UnionsApiController(ITeamsManager teamsManager)
        {
            _teamsManager = teamsManager;
        }

        #region POST

        [Route("api/unions/{unionId}/teams/{teamId}/copy")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPost]
        public IHttpActionResult Copy([FromUri]string unionId, [FromUri]string teamId)
        {
            var model = _teamsManager.Copy(teamId, unionId);
            return Json(model);
        }

        #endregion
    }
}

using System.Linq;
using System.Web.Http;
using DiySoccer.Core.Attributes;
using Interfaces.Core;
using Interfaces.Users.BuisnessLogic;

namespace DiySoccer.Api
{
    public class UsersController : BaseApiController
    {
        private readonly IUsersManager _usersManager;

        public UsersController(IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        #region GET

        [Route("api/league/{leagueId}/users/")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult FindPlayer(string leagueId, string query, int page, int pageSize, string exceptTeamIds = null)
        {
            var exceptTeamIdsList = string.IsNullOrEmpty(exceptTeamIds) ? Enumerable.Empty<string>() : exceptTeamIds.Split(',').Where(x => !string.IsNullOrEmpty(x));
            var results = _usersManager.FindPlayer(leagueId, query, exceptTeamIdsList, page, pageSize);
            return Json(results);
        }

        [Route("api/users/")]
        [DiySoccerAuthorize(LeagueAccessStatus.Admin)]
        [HttpGet]
        public IHttpActionResult FindUser(string query, int page, int pageSize)
        {
            var results = _usersManager.FindUser(query, page, pageSize);
            return Json(results);
        }

        #endregion
    }
}

using System.Linq;
using System.Web.Http;
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
        [HttpGet]
        public IHttpActionResult Find(string leagueId, string query, string exceptTeamIds, int page, int pageSize)
        {
            var exceptTeamIdsList = string.IsNullOrEmpty(exceptTeamIds) ? Enumerable.Empty<string>() : exceptTeamIds.Split(',').Where(x => !string.IsNullOrEmpty(x));
            var results = _usersManager.Find(leagueId, query, exceptTeamIdsList, page, pageSize);
            return Json(results);
        }

        #endregion
    }
}
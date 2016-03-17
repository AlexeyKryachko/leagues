
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

        /*[Route("api/league/{leagueId}/teams/{teamId}")]
        [HttpGet]
        public IHttpActionResult GetTeamByLeague(string leagueId, string teamId)
        {
        }*/

        #endregion
    }
}
using System.Linq;
using System.Web.Http;
using Interfaces.Teams.BuisnessLogic;
using Interfaces.Teams.BuisnessLogic.Models;

namespace DiySoccer.Api
{
    public class TeamsController : BaseApiController
    {
        private readonly ITeamsManager _teamsManager;

        public TeamsController(ITeamsManager teamsManager)
        {
            _teamsManager = teamsManager;
        }

        [Route("api/teams/getTeamsByLeague/{leagueId}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult Get(string leagueId)
        {
            var teams = _teamsManager.GetByLeague(leagueId).ToList();
            return Json(teams);
        }

        [HttpPost]
        public void Create([FromBody]CreateTeamViewModel model)
        {
            _teamsManager.Create(model);
        }
    }
}
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
        [HttpGet]
        public IHttpActionResult Get(string leagueId)
        {
            var teams = _teamsManager.GetByLeague(leagueId).ToList();
            return Json(teams);
        }

        [Route("api/teams/statistic/{leagueId}")]
        [HttpGet]
        public IHttpActionResult GetStatistic(string leagueId)
        {
            var statistic = _teamsManager.GetStatisticByLeague(leagueId).ToList();
            return Json(statistic);
        }

        [HttpPost]
        public void Create([FromBody]CreateTeamViewModel model)
        {
            _teamsManager.Create(model);
        }
    }
}
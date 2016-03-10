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

        #region GET

        [Route("api/league/{leagueId}/teams/{teamId}")]
        [HttpGet]
        public IHttpActionResult GetTeamsByLeague(string leagueId, string teamId)
        {
            var team = _teamsManager.Get(leagueId, teamId);
            return Json(team);
        }

        [Route("api/teams/getTeamsByLeague/{leagueId}")]
        [HttpGet]
        public IHttpActionResult GetTeamsByLeague(string leagueId)
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

        #endregion

        #region PUT

        [Route("api/leagues/{leagueId}/teams/{teamId}")]
        [HttpPut]
        public IHttpActionResult Update([FromUri]string leagueId, [FromUri]string teamId, [FromBody]CreateTeamViewModel model)
        {
            _teamsManager.Update(leagueId, teamId, model);
            return Ok();
        }

        #endregion

        [Route("api/leagues/{leagueId}/teams")]
        [HttpPost]
        public void Create([FromUri]string leagueId, [FromBody]CreateTeamViewModel model)
        {
            _teamsManager.Create(leagueId, model);
        }
    }
}
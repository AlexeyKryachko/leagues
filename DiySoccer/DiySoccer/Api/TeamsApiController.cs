using System.Linq;
using System.Web.Http;
using DiySoccer.Core.Attributes;
using Interfaces.Core;
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

        [Route("api/leagues/{leagueId}/teams/{teamId}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetTeamByLeague(string leagueId, string teamId)
        {
            var team = _teamsManager.Get(leagueId, teamId);
            return Json(team);
        }

        [Route("api/leagues/{leagueId}/teams/{teamId}/info")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetTeamInfo(string leagueId, string teamId)
        {
            var teamInfo = _teamsManager.GetInfo(leagueId, teamId);
            return Json(teamInfo);
        }

        [Route("api/leagues/{leagueId}/teams")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetTeamsByLeague(string leagueId)
        {
            var teams = _teamsManager.GetByLeague(leagueId).ToList();
            return Json(teams);
        }

        [Route("api/teams/search")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult FindTeams(string query, int page, int pageSize)
        {
            var teams = _teamsManager.Find(query, page, pageSize).ToList();
            return Json(teams);
        }

        #endregion

        #region PUT

        [Route("api/leagues/{leagueId}/teams/{teamId}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPut]
        public IHttpActionResult Update([FromUri]string leagueId, [FromUri]string teamId, [FromBody]TeamViewModel model)
        {
            _teamsManager.Update(leagueId, teamId, model);
            return Json(model);
        }

        #endregion

        #region POST

        [Route("api/leagues/{leagueId}/teams")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPost]
        public IHttpActionResult Create([FromUri]string leagueId, [FromBody]TeamViewModel model)
        {
            _teamsManager.Create(leagueId, model);
            return Json(model);
        }

        #endregion
    }
}
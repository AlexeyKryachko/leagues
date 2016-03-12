using System.Linq;
using System.Web.Http;
using Interfaces.Teams.BuisnessLogic;

namespace DiySoccer.Api
{
    public class LeaguesApiController : BaseApiController
    {
        private readonly ITeamsManager _teamsManager;

        public LeaguesApiController(ITeamsManager teamsManager)
        {
            _teamsManager = teamsManager;
        }

        #region GET

        [Route("api/leagues/{leagueId}/statistic")]
        [HttpGet]
        public IHttpActionResult GetStatistic(string leagueId)
        {
            var statistic = _teamsManager.GetStatisticByLeague(leagueId).ToList();
            return Json(statistic);
        }

        #endregion

        #region PUT
        
        #endregion
    }
}
using System.Linq;
using System.Web.Http;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Teams.BuisnessLogic;

namespace DiySoccer.Api
{
    public class LeaguesApiController : BaseApiController
    {
        private readonly ILeaguesManager _leaguesManager;

        public LeaguesApiController(ILeaguesManager leaguesManager)
        {
            _leaguesManager = leaguesManager;
        }

        #region GET

        [Route("api/leagues/{leagueId}/statistic")]
        [HttpGet]
        public IHttpActionResult GetStatistic(string leagueId)
        {
            var statistic = _leaguesManager.GetStatisticByLeague(leagueId);
            return Json(statistic);
        }

        [Route("api/leagues/{leagueId}")]
        [HttpGet]
        public IHttpActionResult Get(string leagueId)
        {
            var league = _leaguesManager.Get(leagueId);
            return league != null
                ? Json(league)
                : null;
        }

        [Route("api/leagues")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var league = _leaguesManager.GetAll();
            return Json(league);
        }

        #endregion

        #region PUT

        [Route("api/leagues/{leagueId}")]
        [HttpPut]
        public IHttpActionResult Update(LeagueViewModel model)
        {
            _leaguesManager.Update(model);

            return Json(model);
        }

        #endregion

        #region POST

        [Route("api/leagues")]
        [HttpPost]
        public IHttpActionResult Create(LeagueViewModel model)
        {
            _leaguesManager.Create(model);

            return Json(model);
        }

        #endregion
    }
}
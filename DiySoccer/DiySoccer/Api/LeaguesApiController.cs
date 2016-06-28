using System.Web.Http;
using DiySoccer.Core.Attributes;
using Interfaces.Core;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Leagues.BuisnessLogic.Model;

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
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetStatistic(string leagueId)
        {
            var statistic = _leaguesManager.GetStatisticByLeague(leagueId);
            return Json(statistic);
        }

        [Route("api/leagues/{leagueId}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Admin)]
        [HttpGet]
        public IHttpActionResult GetUnsecure(string leagueId)
        {
            var league = _leaguesManager.GetUnsecure(leagueId);
            return Json(league);
        }

        [Route("api/leagues/{leagueId}/info")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetInfo(string leagueId)
        {
            var league = _leaguesManager.LeagueInfoViewModel(leagueId);
            return Json(league);
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
        [DiySoccerAuthorize(LeagueAccessStatus.Admin)]
        [HttpPut]
        public IHttpActionResult Update(LeagueUnsecureViewModel model)
        {
            _leaguesManager.Update(model);

            return Json(model);
        }

        #endregion

        #region POST

        [Route("api/leagues")]
        [DiySoccerAuthorize(LeagueAccessStatus.Admin)]
        [HttpPost]
        public IHttpActionResult Create(LeagueUnsecureViewModel model)
        {
            _leaguesManager.Create(model);

            return Json(model);
        }

        #endregion
    }
}
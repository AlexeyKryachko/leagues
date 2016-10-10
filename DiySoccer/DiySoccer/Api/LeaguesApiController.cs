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

        [Route("api/leagues/{leagueId}/statistics")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetLeagueStatistic(string leagueId)
        {
            var statistic = _leaguesManager.GetStatisticByLeague(leagueId);
            return Json(statistic);
        }

        [Route("api/leagues/{leagueId}/table")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetTable(string leagueId)
        {
            var statistic = _leaguesManager.GetTable(leagueId);
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
            var league = _leaguesManager.GetInfo(leagueId);
            return Json(league);
        }

        [Route("api/leagues")]
        [HttpGet]
        public IHttpActionResult GetLeagues()
        {
            var league = _leaguesManager.GetLeagues();
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

        #region DELETE

        [Route("api/leagues/{id}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Admin)]
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            _leaguesManager.Delete(id);
            return Ok();
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Web.Http;
using DiySoccer.Core.Attributes;
using Interfaces.Core;
using Interfaces.Events.BuisnessLogic;

namespace DiySoccer.Api
{
    public class CalendarApiController : BaseApiController
    {
        private readonly IEventsManager _eventsManager;

        public CalendarApiController(IEventsManager eventsManager)
        {
            _eventsManager = eventsManager;
        }

        #region GET
        
        [Route("api/leagues/{leagueId}/events")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetCalendar(string leagueId)
        {
            var model = _eventsManager.GetRange(leagueId);
            return Json(model);
        }

        #endregion

        #region PUT
        
        #endregion

        #region POST
        #endregion

        #region DELETE
        
        #endregion
    }
}
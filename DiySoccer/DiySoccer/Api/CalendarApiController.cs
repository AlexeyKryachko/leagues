
using System.Collections.Generic;
using System.Web.Http;
using DiySoccer.Core.Attributes;
using Interfaces.Calendar;
using Interfaces.Core;
using Interfaces.Events.BuisnessLogic;

namespace DiySoccer.Api
{
    public class CalendarApiController : BaseApiController
    {
        private readonly IEventsManager _eventsManager;
        private readonly ICalendarManager _calendarManager;

        public CalendarApiController(IEventsManager eventsManager, ICalendarManager calendarManager)
        {
            _eventsManager = eventsManager;
            _calendarManager = calendarManager;
        }

        #region GET
        
        [Route("api/leagues/{leagueId}/calendar")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetCalendar(string leagueId)
        {
            var model = _calendarManager.GetViewModel(leagueId);
            return Json(model);
        }

        #endregion

        #region PUT

        #endregion

        #region POST

        [Route("api/leagues/{leagueId}/events")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPost]
        public IHttpActionResult Create([FromUri]string leagueId)
        {
            var model = _eventsManager.Create(leagueId);
            return Json(model);
        }

        #endregion

        #region DELETE

        [Route("api/leagues/{leagueId}/events/{eventId}")]
        [HttpDelete]
        public void Delete(string leagueId, string eventId)
        {
            _eventsManager.Delete(eventId);
        }

        #endregion
    }
}
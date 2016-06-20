
using System.Collections.Generic;
using System.Web.Http;
using DiySoccer.Core.Attributes;
using Interfaces.Calendar;
using Interfaces.Core;
using Interfaces.Events.BuisnessLogic;
using Interfaces.Events.BuisnessLogic.Models;

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

        [Route("api/leagues/{leagueId}/events")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetEvents(string leagueId)
        {
            var model = _calendarManager.GetEvents(leagueId);
            return Json(model);
        }

        #endregion

        #region PUT

        [Route("api/leagues/{leagueId}/events/{eventId}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPut]
        public IHttpActionResult Update([FromUri]string leagueId, [FromBody]EventVewModel model)
        {
            var updated = _eventsManager.Update(leagueId, model);
            return Json(updated);
        }

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

        [Route("api/leagues/{leagueId}/events/{eventId}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpPost]
        public IHttpActionResult CreateEventGame([FromUri]string leagueId, [FromUri]string eventId)
        {
            var model = _eventsManager.CreateEventGame(leagueId, eventId);
            return Json(model);
        }

        #endregion

        #region DELETE

        [Route("api/leagues/{leagueId}/events/{eventId}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpDelete]
        public void Delete(string leagueId, string eventId)
        {
            _eventsManager.Delete(eventId);
        }

        [Route("api/leagues/{leagueId}/events/{eventId}/{eventGameId}")]
        [DiySoccerAuthorize(LeagueAccessStatus.Editor)]
        [HttpDelete]
        public void Delete(string leagueId, string eventId, int eventGameId)
        {
            _eventsManager.DeleteEventGame(eventId, eventGameId);
        }

        #endregion
    }
}
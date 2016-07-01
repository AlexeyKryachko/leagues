using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DiySoccer.Core.Attributes;
using Interfaces.Core;
using Interfaces.Unions.BuisnessLogic;

namespace DiySoccer.Api
{
    public class TournamentsApiController : BaseApiController
    {
        private readonly ITournamentsManager _tournamentsManager;

        public TournamentsApiController(ITournamentsManager tournamentsManager)
        {
            _tournamentsManager = tournamentsManager;
        }

        #region GET

        [Route("api/tournaments/{tournamentId}/info")]
        [DiySoccerAuthorize(LeagueAccessStatus.Member)]
        [HttpGet]
        public IHttpActionResult GetInfo(string tournamentId)
        {
            var model = _tournamentsManager.GetTournamentInfo(tournamentId);
            return Json(model);
        }

        #endregion
    }
}
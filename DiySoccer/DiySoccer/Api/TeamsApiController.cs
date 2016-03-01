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

        [HttpGet]
        public void Hello()
        {
        }

        [HttpPost]
        public void Create([FromBody]CreateTeamViewModel model)
        {
            _teamsManager.Create(model);
        }
    }
}
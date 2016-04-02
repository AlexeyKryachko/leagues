
using System.Web.Http;
using System.Web.Http.Controllers;
using Interfaces.Authenticate.BuisnessLogic;
using Interfaces.Core;

namespace DiySoccer.Core.Attributes
{
    public class DiySoccerAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly LeagueAccessStatus _accessStatus;

        public IAuthenticateManager AuthenticateManager { get; set; }

        public DiySoccerAuthorizeAttribute(LeagueAccessStatus accessStatus)
        {
            _accessStatus = accessStatus;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var leagueValue = actionContext.ControllerContext.RouteData.Values["leagueId"];
            var leagueId = leagueValue == null ? string.Empty : leagueValue.ToString();

            switch (_accessStatus)
            {
                case LeagueAccessStatus.Member:
                    return AuthenticateManager.IsMember(leagueId);
                case LeagueAccessStatus.Editor:
                    return AuthenticateManager.IsEditor(leagueId);
                case LeagueAccessStatus.Admin:
                    return AuthenticateManager.IsAdmin();
                default:
                    return false;
            }
        }
    }
}
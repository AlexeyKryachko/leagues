
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace DiySoccer.Core.Attributes
{
    public class AdminRoleAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
                return false;

            return true;
        }
    }
}
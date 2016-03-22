using System;
using System.Diagnostics;
using System.Web.Mvc;
using Interfaces.Core.Authentication;
using Interfaces.Users.BuisnessLogic;

namespace DiySoccer.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DiySoccerAuthorizeAttribute : BaseAttribute, IAuthorizationFilter
    {
        private readonly Roles[] _acceptedRoles;

        public IUsersManager UsersManager; 

        public DiySoccerAuthorizeAttribute(params Roles[] acceptedRoles)
        {
            _acceptedRoles = acceptedRoles;   
        }
     
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //var provider = filterContext.RequestContext.HttpContext.Request.Headers["authProvider"];

            Debug.WriteLine("Test authenticated");
        }
    }
}

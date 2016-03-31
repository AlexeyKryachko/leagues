using System.Web;
using System.Web.Http;

namespace DiySoccer.Api
{
    public class AuthenticationApiController : BaseApiController
    {
        #region GET

        [Route("api/logout")]
        [HttpGet]
        public void Logout()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut();
        }

        #endregion
    }
}
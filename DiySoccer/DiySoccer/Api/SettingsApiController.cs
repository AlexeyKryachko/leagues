using System.Web.Http;
using Interfaces.Authenticate.BuisnessLogic;
namespace DiySoccer.Api
{
    public class SettingsApiController : BaseApiController
    {
        private readonly IAuthenticateManager _authenticateManager;

        public SettingsApiController(IAuthenticateManager authenticateManager)
        {
            _authenticateManager = authenticateManager;
        }

        #region GET

        [Route("api/settings")]
        [HttpGet]
        public IHttpActionResult GetSettings()
        {
            var model = _authenticateManager.GetSettings();
            return Json(model);
        }
        
        #endregion

    }
}
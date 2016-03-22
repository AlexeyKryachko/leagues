
using System.Collections.Generic;
using System.Web.Http;
using Interfaces.Games.BuisnessLogic;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Settings.BuisnessLogic;

namespace DiySoccer.Api
{
    public class SettingsApiController : BaseApiController
    {
        #region GET

        [Route("api/settings")]
        [HttpGet]
        public IHttpActionResult GetSettings()
        {
            var model = new SettingsViewModel
            {
                Permissions = new PermissionsViewModel
                {
                    Relationships = new Dictionary<string, string>() { { "1", "2" } }
                }
            };
            return Json(model);
        }

        #endregion
        
    }
}
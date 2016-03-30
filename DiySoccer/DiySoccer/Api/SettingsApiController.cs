
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Authentication;
using Interfaces.Settings.BuisnessLogic;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using VkNet;

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

            var userId = User.Identity.GetUserId();

            var api = new VkApi();

            //bool isMember = new VkApi().Groups.IsMember(27134671);

            return Json(model);
        }

        private ApplicationUser GetCurrentUser(ApplicationIdentityContext context)
        {
            var identity = User.Identity as ClaimsIdentity;
            Claim identityClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            return context.Users.AsQueryable().FirstOrDefault(u => u.Id == identityClaim.Value);
        }

        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Authentication;
using Interfaces.Core;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Settings.BuisnessLogic;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MongoDB.Driver;
using VkNet;

namespace DiySoccer.Api
{
    public class SettingsApiController : BaseApiController
    {
        private readonly ILeaguesManager _leaguesManager;

        public SettingsApiController(ILeaguesManager leaguesManager)
        {
            _leaguesManager = leaguesManager;
        }

        #region GET

        [Route("api/settings")]
        [HttpGet]
        public IHttpActionResult GetSettings()
        {
            var model = new SettingsViewModel();

            model.Permissions.IsAuthenticated = User.Identity.IsAuthenticated;
            model.Permissions.IsAdmin = User.Identity.IsAuthenticated && User.Identity.Name == "alexey.kryachko@gmail.com";

            var relationships = new Dictionary<string, string>();
            var leagues = _leaguesManager.GetAllUnsecure();
            foreach (var league in leagues)
            {
                if (model.Permissions.IsAdmin)
                {
                    relationships.Add(league.Id, ((int)LeagueAccessStatus.Editor).ToString());
                    continue;
                }

                if (string.IsNullOrEmpty(league.VkGroup))
                {
                    relationships.Add(league.Id, ((int)LeagueAccessStatus.Member).ToString());
                    continue;
                }

                var context = HttpContext.Current.GetOwinContext();
                var loginInfo = context.Authentication.GetExternalLoginInfoAsync();
                if (loginInfo == null || loginInfo.Result == null)
                {
                    relationships.Add(league.Id, ((int)LeagueAccessStatus.Undefined).ToString());
                    continue;
                }

                var vkUserIdClaim = loginInfo.Result.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "VkUserId");
                if (vkUserIdClaim == null)
                {
                    relationships.Add(league.Id, ((int)LeagueAccessStatus.Undefined).ToString());
                    continue;
                }

                var api = new VkApi();
                long vkUserId;
                try
                {
                    vkUserId = long.Parse(vkUserIdClaim.Value);
                }
                catch (Exception)
                {
                    relationships.Add(league.Id, ((int)LeagueAccessStatus.Undefined).ToString());
                    continue;
                }
                
                var response = api.Groups.IsMember("spbdiyfootball", vkUserId, new long[] { vkUserId }, false);
                if (!response[0].Member)
                {
                    relationships.Add(league.Id, ((int)LeagueAccessStatus.Undefined).ToString());
                }

                relationships.Add(league.Id, ((int)LeagueAccessStatus.Member).ToString());
            }

            model.Permissions.Relationships = relationships;
            
            return Json(model);
        }
        
        #endregion

    }
}
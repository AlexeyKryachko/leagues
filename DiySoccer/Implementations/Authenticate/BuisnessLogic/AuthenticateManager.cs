using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interfaces.Authenticate.BuisnessLogic;
using Interfaces.Core;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Settings.BuisnessLogic;
using Microsoft.Owin.Security;
using VkNet;

namespace Implementations.Authenticate.BuisnessLogic
{
    public class AuthenticateManager : IAuthenticateManager
    {
        private readonly ILeaguesManager _leaguesManager;

        public AuthenticateManager(ILeaguesManager leaguesManager)
        {
            _leaguesManager = leaguesManager;
        }

        public SettingsViewModel GetSettings()
        {
            var model = new SettingsViewModel();

            var authUser = HttpContext.Current.User;
            model.Permissions.IsAuthenticated = authUser.Identity.IsAuthenticated;
            model.Permissions.IsAdmin = IsAdmin();

            var relationships = new Dictionary<string, string>();
            var leagues = _leaguesManager.GetAllUnsecure();
            foreach (var league in leagues)
            {
                var access = GetAccess(league);
                relationships.Add(league.Id, ((int)access).ToString());
            }

            model.Permissions.Relationships = relationships;

            return model;
        }

        private LeagueAccessStatus GetAccess(LeagueUnsecureViewModel league)
        {
            if (IsAdmin())
                return LeagueAccessStatus.Admin;

            //check if user is editor for none vk group

            if (string.IsNullOrEmpty(league.VkGroup))
                return LeagueAccessStatus.Member;

            var context = HttpContext.Current.GetOwinContext();
            var loginInfo = context.Authentication.GetExternalLoginInfoAsync();
            if (loginInfo == null || loginInfo.Result == null)
                return LeagueAccessStatus.Undefined;

            var vkUserIdClaim = loginInfo.Result.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "VkUserId");
            if (vkUserIdClaim == null)
                return LeagueAccessStatus.Undefined;

            var api = new VkApi();
            long vkUserId;
            try
            {
                vkUserId = long.Parse(vkUserIdClaim.Value);
            }
            catch (Exception)
            {
                return LeagueAccessStatus.Undefined;
            }

            var response = api.Groups.IsMember("spbdiyfootball", vkUserId, new long[] { vkUserId }, false);
            if (!response[0].Member)
                return LeagueAccessStatus.Undefined;

            //checked IFormatProvider user is VkApi editor

            return LeagueAccessStatus.Member;
        }
        
        public bool IsEditor(string leagueId)
        {
            if (IsAdmin())
                return true;

            var league = _leaguesManager.GetUnsecure(leagueId);
            if (league == null)
                return false;

            var access = GetAccess(league);
            return access == LeagueAccessStatus.Editor || access == LeagueAccessStatus.Admin;
        }

        public bool IsAdmin()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity.Name == "alexey.kryachko@gmail.com";
        }
    }
}

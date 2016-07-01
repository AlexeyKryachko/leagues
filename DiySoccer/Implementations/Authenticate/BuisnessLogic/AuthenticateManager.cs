using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interfaces.Authenticate.BuisnessLogic;
using Interfaces.Core;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Settings.BuisnessLogic;
using Interfaces.Users.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using VkNet;

namespace Implementations.Authenticate.BuisnessLogic
{
    public class AuthenticateManager : IAuthenticateManager
    {
        private readonly ILeaguesManager _leaguesManager;
        private readonly IUsersRepository _usersRepository;

        public AuthenticateManager(ILeaguesManager leaguesManager, IUsersRepository usersRepository)
        {
            _leaguesManager = leaguesManager;
            _usersRepository = usersRepository;
        }

        public SettingsViewModel GetSettings()
        {
            var model = new SettingsViewModel();
            
            model.Permissions.IsAdmin = IsAdmin();

            var context = HttpContext.Current.GetOwinContext();
            var externalIdentity = context.Authentication.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
            model.Permissions.IsAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated ||
                (externalIdentity != null && externalIdentity.IsAuthenticated);

            var currentUserId = GetCurrentUserId(context);

            var relationships = new Dictionary<string, string>();
            var leagues = _leaguesManager.GetAllUnsecure();
            foreach (var league in leagues)
            {
                var access = GetAccess(league, context, currentUserId);
                relationships.Add(league.Id, ((int)access).ToString());
            }

            model.Permissions.Relationships = relationships;

            return model;
        }

        public string GetCurrentUserId(IOwinContext context)
        {
            var id = HttpContext.Current.User.Identity.GetUserId();
            if (id == null)
            {
                var externalIdentity = context.Authentication.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                if (externalIdentity != null)
                {
                    var user = _usersRepository.GetByUserName(externalIdentity.Name);
                    if (user != null)
                        id = user.Id;
                }
            }

            return id;
        }

        private LeagueAccessStatus GetAccess(LeagueUnsecureViewModel league, IOwinContext context, string currentUserId)
        {
            if (IsAdmin())
                return LeagueAccessStatus.Admin;
            
            if (string.IsNullOrEmpty(league.VkGroup))
            {
                return league.Admins.Any(x => x.Id == currentUserId)
                    ? LeagueAccessStatus.Editor
                    : LeagueAccessStatus.Member;
            }

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
            
            return league.Admins.Any(x => x.Id == currentUserId)
                ? LeagueAccessStatus.Editor
                : LeagueAccessStatus.Member;
        }

        public bool IsMember(string unionId)
        {
            if (IsAdmin())
                return true;

            var union = _leaguesManager.GetUnsecure(unionId);
            if (union == null)
                return false;

            var context = HttpContext.Current.GetOwinContext();
            var currentUserId = GetCurrentUserId(context);

            var access = GetAccess(union, context, currentUserId);
            return access == LeagueAccessStatus.Member ||
                access == LeagueAccessStatus.Editor || 
                access == LeagueAccessStatus.Admin;
        }

        public bool IsEditor(string leagueId)
        {
            if (IsAdmin())
                return true;

            var league = _leaguesManager.GetUnsecure(leagueId);
            if (league == null)
                return false;

            var context = HttpContext.Current.GetOwinContext();
            var currentUserId = GetCurrentUserId(context);

            var access = GetAccess(league, context, currentUserId);
            return access == LeagueAccessStatus.Editor || access == LeagueAccessStatus.Admin;
        }

        public bool IsAdmin()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity.Name == "alexey.kryachko@gmail.com";
        }
    }
}

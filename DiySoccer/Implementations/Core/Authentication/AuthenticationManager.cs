using System.Net;
using Interfaces.Core.Authentication;
using Newtonsoft.Json.Linq;

namespace Implementations.Core.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private const string AppId = "5370269";
        private const string SecretKey = "RMw7P2WLOypLGgzc7fzi";
        private const string RedirectUrl = "http://diysoccer.azurewebsites.net/home/authvk/";
        
        public JObject AuthenticateThroughVk(string code)
        {
            WebClient request = new WebClient();
            var url = "https://oauth.vk.com/access_token?client_id=" + AppId + "&client_secret=" + SecretKey + "&code=" + code + "&redirect_uri=" + RedirectUrl;
            var response = request.DownloadString(url);

            return JObject.Parse(response);
        }
    }
}

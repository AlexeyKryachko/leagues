using Newtonsoft.Json.Linq;

namespace Interfaces.Core.Authentication
{
    public interface IAuthenticationManager
    {
        JObject AuthenticateThroughVk(string code);
    }
}

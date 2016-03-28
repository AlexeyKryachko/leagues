using System.Web.Http;
using System.Web.Http.Cors;

namespace DiySoccer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();

            config.MapHttpAttributeRoutes();
        }
    }
}

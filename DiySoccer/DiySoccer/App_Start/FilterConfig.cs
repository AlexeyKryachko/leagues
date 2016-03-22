using System.Web.Mvc;
using DiySoccer.Attributes;

namespace DiySoccer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new DiySoccerAuthorizeAttribute());
        }
    }
}
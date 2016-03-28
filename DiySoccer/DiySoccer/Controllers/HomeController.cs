using System;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using DiySoccer.Core;
using Interfaces.Core.Authentication;

namespace DiySoccer.Controllers
{
    [EnableCors("*", "*", "*")]
    public class HomeController : Controller
    {
        private readonly IAuthenticationManager _authenticationManager;

        public HomeController(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }
        
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult AuthVk(string code)
        {
            var client = new VKontakteAuthenticationClient();
            var httpContextBase = new HttpContextWrapper(System.Web.HttpContext.Current);

            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    var url = new Uri("http://diysoccer.azurewebsites.net/Home/AuthVk");

                    client.RequestAuthentication(httpContextBase, url);
                }
                else
                {
                    var test = client.VerifyAuthentication(httpContextBase);
                }
            }
            catch (Exception)
            {
                
            }

            return Redirect("/");
        }
    }
}

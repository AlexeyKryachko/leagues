using System.Web.Mvc;
using System.Web.Http.Cors;

namespace DiySoccer.Controllers
{
    [EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }        
    }
}

using System.Web.Mvc;
using System.Web.Http.Cors;
using Interfaces.Teams.BuisnessLogic;

namespace DiySoccer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITeamsManager _teamsManager;

        public HomeController(ITeamsManager teamsManager)
        {
            _teamsManager = teamsManager;
        }

        public ActionResult Index()
        {
            return View();
        }        
    }
}

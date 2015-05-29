using System.Web.Mvc;

namespace CrescentIsland.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
using CrescentIsland.Website.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace CrescentIsland.Website.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
                return View("AltIndex");
            else
                return View();
        }

        public ActionResult HeaderPartial(HeaderModel model)
        {
            return PartialView("_Header");
        }
    }
}
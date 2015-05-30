using CrescentIsland.Website.Models;
using System.Web.Mvc;

namespace CrescentIsland.Website.Controllers
{
    public class PageController : Controller
    {
        //
        // GET: /Page/Index
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated) return View();

            return View("AltIndex");
        }

        //
        // GET: /Page/Character
        public ActionResult Character()
        {
            if (!Request.IsAuthenticated) return View("Index");

            return View();
        }

        //
        // GET: /Page/Inventory
        public ActionResult Inventory()
        {
            if (!Request.IsAuthenticated) return View("Index");

            return View();
        }

        //
        // GET: /Page/Lucys
        public ActionResult Lucys()
        {
            if (!Request.IsAuthenticated) return View("Index");

            return View();
        }

        //
        // GET: /Page/Axebeards
        public ActionResult Axebeards()
        {
            if (!Request.IsAuthenticated) return View("Index");

            return View();
        }

        //
        // GET: /Page/LastSpell
        public ActionResult LastSpell()
        {
            if (!Request.IsAuthenticated) return View("Index");

            return View();
        }

        public ActionResult HeaderPartial(HeaderModel model)
        {
            return PartialView("_Header");
        }
    }
}
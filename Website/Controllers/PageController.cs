using CrescentIsland.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace CrescentIsland.Website.Controllers
{
    public class PageController : Controller
    {
        private ApplicationUserManager _userManager;

        public PageController()
        {
        }
        public PageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Page/Index
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated) return View();

            return View("AltIndex");
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

        public ActionResult HeaderPartial()
        {
            var model = new HeaderModel();

            if (HttpContext.Request.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());

                if (user != null)
                {
                    model.AvatarImage = user.AvatarImage;
                    model.AvatarMimeType = user.AvatarMimeType;
                    model.UserRoles = user.Roles;
                    model.Username = user.UserName;
                    model.Level = user.Level;
                    model.CurHealth = user.CurHealth;
                    model.MaxHealth = user.MaxHealth;
                    model.CurEnergy = user.CurEnergy;
                    model.MaxEnergy = user.MaxEnergy;
                }
            }

            return PartialView("_Header", model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}
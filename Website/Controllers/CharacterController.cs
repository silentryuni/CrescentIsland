using CrescentIsland.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CrescentIsland.Website.Controllers
{
    public class CharacterController : Controller
    {
        private ApplicationUserManager _userManager;

        public CharacterController()
        {
        }
        public CharacterController(ApplicationUserManager userManager)
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
        // GET: /Character/{username}
        public async Task<ActionResult> Index(string username)
        {
            if (!Request.IsAuthenticated) return RedirectToAction("Index", "Page");

            var model = new CharacterViewModel();
            var user = await UserManager.FindByNameAsync(username);

            if (user == null)
            {
                model.CharacterFound = false;
                return View(model);
            }

            model.CharacterFound = true;
            model.CharacterName = user.UserName;
            model.UserClass = user.UserClass;
            model.Level = user.Level;
            model.CurExp = user.CurExp;
            model.MaxExp = user.MaxExp;
            model.Gold = user.Gold;
            model.CurHealth = user.CurHealth;
            model.MaxHealth = user.MaxHealth;
            model.CurEnergy = user.CurEnergy;
            model.MaxEnergy = user.MaxEnergy;
            model.Attack = user.Attack;
            model.Defense = user.Defense;
            model.MagicAttack = user.MagicAttack;
            model.MagicDefense = user.MagicDefense;
            model.Accuracy = user.Accuracy;
            model.Evasion = user.Evasion;

            return View(model);
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
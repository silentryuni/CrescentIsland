using CrescentIsland.Website.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CrescentIsland.Website.Controllers
{
    public class CharacterController : Controller
    {
        private ApplicationUserManager _userManager;
        private CharacterManager _charManager;

        public CharacterController()
        {
        }
        public CharacterController(ApplicationUserManager userManager, CharacterManager charManager)
        {
            UserManager = userManager;
            CharManager = charManager;
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
        public CharacterManager CharManager
        {
            get
            {
                return _charManager ?? HttpContext.GetOwinContext().Get<CharacterManager>();
            }
            private set
            {
                _charManager = value;
            }
        }

        //
        // GET: /Character/{username}
        public async Task<ActionResult> Index(string charname)
        {
            if (!Request.IsAuthenticated) return RedirectToAction("Index", "Page");

            var model = new CharacterViewModel();
            var character = await CharManager.FindCharacterAsync(charname);

            if (character == null)
            {
                model.CharacterFound = false;
                return View(model);
            }

            model.CharacterFound = true;
            model.CharacterName = character.CharacterName;
            model.UserClass = character.UserClass;
            model.Level = character.Level;
            model.CurExp = character.CurExp;
            model.MaxExp = character.MaxExp;
            model.Gold = character.Gold;
            model.CurHealth = character.CurHealth;
            model.MaxHealth = character.MaxHealth;
            model.CurEnergy = character.CurEnergy;
            model.MaxEnergy = character.MaxEnergy;
            model.Attack = character.Attack;
            model.Defense = character.Defense;
            model.MagicAttack = character.MagicAttack;
            model.MagicDefense = character.MagicDefense;
            model.Accuracy = character.Accuracy;
            model.Evasion = character.Evasion;

            return View(model);
        }

        public ActionResult Inventory()
        {
            return View();
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
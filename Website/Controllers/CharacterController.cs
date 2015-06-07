using CrescentIsland.Website.Helpers;
using CrescentIsland.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
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

            var user = await CharManager.FindCharacterOwnerAsync(charname);
            if (user == null)
            {
                model.UserFound = false;
                return View(model);
            }

            model.UserFound = true;
            model.Username = user.UserName;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Gender = (user.ShowGender ? user.UserGender.ToString() : hiddenValue);
            model.Age = (user.ShowAge ? CalculateAge(user.Birthday).ToString() : hiddenValue);
            model.Country = user.Country;
            model.Biography = user.Biography;
            model.AvatarImage = user.AvatarImage;
            model.AvatarMimeType = user.AvatarMimeType;
            model.AccountAge = AccountAge(user.Created);
            model.LastLogin = LastLogin(user.LastLogin);

            var character = user.Characters.FirstOrDefault();
            if (character == null)
            {
                model.CharacterFound = false;
                return View(model);
            }

            model.CharacterFound = true;
            model.OwnCharacter = user.Id.Equals(User.Identity.GetUserId());

            model.CharacterName = character.CharacterName;
            model.UserClass = character.UserClass.ToString();
            model.Level = character.Level;
            model.CurExp = character.CurExp;
            model.MaxExp = character.MaxExp;
            model.Gold = (user.ShowMoney ? character.Gold.ToString() : hiddenValue);
            model.CurHealth = character.CurHealth;
            model.MaxHealth = character.MaxHealth;
            model.Attack = character.Attack;
            model.Defense = character.Defense;
            model.MagicAttack = character.MagicAttack;
            model.MagicDefense = character.MagicDefense;
            model.Accuracy = character.Accuracy;
            model.Evasion = character.Evasion;

            return View(model);
        }

        //
        // GET: /Character/User/Index
        public async Task<ActionResult> UserIndex()
        {
            var model = new CharacterUserViewModel();

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                model.CharacterFound = false;
                return View(model);
            }

            var character = user.Characters.FirstOrDefault();
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
        
        //
        // GET: /Character/Inventory
        public ActionResult Inventory()
        {
            return View();
        }

        #region Helpers

        private const string hiddenValue = "<span class=\"value-hidden\">Hidden</span>";

        private int CalculateAge(DateTime birthday)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthday.Year;
            if (birthday > today.AddYears(-age)) age--;

            return age;
        }

        public string AccountAge(DateTime created)
        {
            var totaldays = DaysBetween(created, DateTime.Now);
            var years = totaldays / 365;
            var days = totaldays % 365;

            return string.Format("{0} Years, {1} Days", years, days);
        }

        public string LastLogin(DateTime lastlogin)
        {
            var today = DateTime.Now;
            TimeSpan span = today.Subtract(lastlogin);
            
            if ((int)span.TotalDays == 0 && span.Hours == 0 && span.Minutes == 0 && span.Seconds < 60)
            {
                return string.Format("{0} seconds ago", span.Seconds);
            }
            else if ((int)span.TotalDays == 0 && span.Hours == 0 && span.Minutes < 60)
            {
                return string.Format("{0} minutes ago", span.Minutes);
            }
            else if ((int)span.TotalDays == 0 && span.Hours < 24)
            {
                return string.Format("{0} hours ago", span.Hours);
            }
            else if ((int)span.TotalDays != 0)
            {
                return string.Format("{0} days ago", (int)span.TotalDays);
            }
            else
            {
                return string.Format("Online now");
            }
        }

        public int DaysBetween(DateTime d1, DateTime d2)
        {
            TimeSpan span = d2.Subtract(d1);
            return Math.Abs((int)span.TotalDays);
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
using CrescentIsland.Website.Models;
using CrescentIsland.Website.Models.Interfaces;
using CrescentIsland.Website.Models.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CrescentIsland.Website.Controllers
{
    public class BattleController : Controller
    {
        #region Interface

        private IUserRepository _userRepository;
        public BattleController() : this(new UserRepository())
        { }
        public BattleController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        // GET: Battle
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateHealth(string actionId)
        {
            var model = new BattleModel();

            var healthChange = 0;

            healthChange = new Random((int)DateTime.Now.Ticks).Next(0, 10) - 5;

            model.Success = await _userRepository.UpdateHealth(healthChange);
            if (model.Success) model.CurHealthChange = healthChange;

            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateEnergy(string actionId)
        {
            var model = new BattleModel();

            var energyChange = 0;

            if (actionId == "0") energyChange = -1;
            if (actionId == "1") energyChange = -2;

            model.Success = await _userRepository.UpdateEnergy(energyChange);
            if (model.Success) model.CurEnergyChange = energyChange;

            return Json(model, JsonRequestBehavior.DenyGet);
        }
    }
}
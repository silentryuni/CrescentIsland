using CrescentIsland.Website.Helpers;
using CrescentIsland.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
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
        // GET: /Character/View/{charname}
        public async Task<ActionResult> ViewIndex(string charname)
        {
            if (!Request.IsAuthenticated) return RedirectToAction("Index", "Page");

            var model = new CharacterViewModel();

            var character = await CharManager.FindCharacterAsync(charname);
            if (character == null)
            {
                model.UserFound = false;
                model.CharacterFound = false;
                return View(model);
            }

            model.CharacterFound = true;
            model.CharacterName = character.CharacterName;
            model.UserClass = character.UserClass.ToString();
            model.Level = character.Level;
            model.CurExp = character.CurExp;
            model.MaxExp = character.MaxExp;
            model.Gold = hiddenValue;

            model.CurHealth = character.CurHealth;
            model.MaxHealth = character.MaxHealth;
            model.Attack = character.Attack;
            model.Defense = character.Defense;
            model.MagicAttack = character.MagicAttack;
            model.MagicDefense = character.MagicDefense;
            model.Accuracy = character.Accuracy;
            model.Evasion = character.Evasion;

            model.Age = "";
            model.LastLogin = "";

            var user = character.User;

            if (user == null)
            {
                model.UserFound = false;
                return View(model);
            }

            model.OwnCharacter = user.Id.Equals(User.Identity.GetUserId());
            model.Gold = (user.ShowMoney ? character.Gold.ToString() : hiddenValue);

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
            
            return View(model);
        }

        //
        // GET: /Character/User/
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

            return View("UserIndex", model);
        }
        
        //
        // GET: /Character/Inventory
        public async Task<ActionResult> Inventory()
        {
            var model = new InventoryViewModel()
            {
                Equipment = new InventoryItem[6],
                Bag = new InventoryItem[10]
            };

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return RedirectToAction("Index", "Page");
            }

            var character = user.Characters.FirstOrDefault();
            if (character == null)
            {
                return RedirectToAction("Index", "Page");
            }

            var items = character.Items;

            if (items != null && items.Count > 0)
            {
                var equipment = items.Where(i => i.EquipmentSlot >= 1 && i.EquipmentSlot <= 6);
                var bagitems = items.Where(i => i.BagSlot >= 1 && i.BagSlot <= 10);

                foreach (var item in equipment)
                {
                    if (item == null || item.Item == null) continue;

                    var invItem = new InventoryItem()
                    {
                        Id = item.Id,
                        Name = item.Item.Name,
                        Description = item.Item.Description,
                        Type = item.Item.Type,
                        Quality = item.Item.Quality,
                        LevelRequirement = item.Item.LevelRequirement,
                        Icon = item.Item.Icon
                    };
                    model.Equipment[item.EquipmentSlot - 1] = invItem;
                }

                foreach (var item in bagitems)
                {
                    if (item == null || item.Item == null) continue;

                    var invItem = new InventoryItem()
                    {
                        Id = item.Id,
                        Name = item.Item.Name,
                        Description = item.Item.Description,
                        Type = item.Item.Type,
                        Quality = item.Item.Quality,
                        LevelRequirement = item.Item.LevelRequirement,
                        Icon = item.Item.Icon
                    };
                    model.Bag[item.BagSlot - 1] = invItem;
                }
            }

            return View(model);
        }

        //
        // POST: /Character/UpdateInventory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateInventory(UpdateCharacterInventory model)
        {
            var modified = false;

            if (model.ItemId > 0)
            {
                var useritem = await CharManager.FindUserItemAsync(model.ItemId);
                if (useritem != null)
                {
                    useritem.BagSlot = model.BagSlot;
                    useritem.EquipmentSlot = model.EquipmentSlot;
                    var item = useritem.Item;
                    var chara = useritem.Character;

                    CharManager.Context.Entry(useritem).State = EntityState.Modified;
                    modified = true;
                }
            }

            if (model.SecondItemId > 0)
            {
                var useritem = await CharManager.FindUserItemAsync(model.SecondItemId);
                if (useritem != null)
                {
                    useritem.BagSlot = model.SecondBagSlot;
                    useritem.EquipmentSlot = model.SecondEquipmentSlot;
                    var item = useritem.Item;
                    var chara = useritem.Character;

                    CharManager.Context.Entry(useritem).State = EntityState.Modified;
                    modified = true;
                }
            }

            var result = 0;
            if (modified)
            {
                try {
                    result = await CharManager.Context.SaveChangesAsync();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting  
                            // the current instance as InnerException  
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
            
            return Json(result);
        }

        //
        // POST: /Character/BuyItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> BuyItem(int itemId)
        {
            var goldSpent = "false";

            var item = await CharManager.FindItemAsync(itemId);
            if (item == null) return Json(goldSpent);

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null) return Json(goldSpent);

            var character = user.Characters.FirstOrDefault();
            if (character == null) return Json(goldSpent);

            // If character doesn't have enough money
            if (character.Gold < item.BuyCost)
            {
                return Json("no money");
            }

            // Get all items that are in bag slots
            var items = character.Items.Where(i => i.BagSlot >= 1 && i.BagSlot <= 10).OrderBy(i => i.BagSlot);
            var slot = 0;
            var count = items.Count();

            // If count is 10 or more, inventory is full
            if (count < 10)
            {
                // If character owns any items, start looking for an available slot
                if (count > 0)
                {
                    UserItem previousItem = null;
                    foreach (var ownedItem in items)
                    {
                        // If this is second or later iteration, check if previous item was one number behind current item
                        // As soon as previous statement is false, that means there's an available slot one number after previous item
                        if (previousItem != null && previousItem.BagSlot != ownedItem.BagSlot - 1)
                        {
                            slot = previousItem.BagSlot + 1;
                            break;
                        }
                        // previousItem is null on first iteration, check if first item is at a higher slot than one, if so, add new item to slot one
                        else if (previousItem == null)
                        {
                            if (ownedItem.BagSlot > 1)
                            {
                                slot = 1;
                                break;
                            }
                        }

                        previousItem = ownedItem;
                    }
                    // If all items has been iterated and there still wasn't any available slots, add item to one slot after last one
                    if (slot == 0)
                    {
                        slot = previousItem.BagSlot + 1;
                    }
                }
                // If character had no items at all in the bag, add new item to slot 1
                else
                {
                    slot = 1;
                }

                var result = 0;
                var useritem = new UserItem()
                {
                    Character = character,
                    BagSlot = slot,
                    EquipmentSlot = 0,
                    Item = item
                };
                // Add useritem to the database
                CharManager.Context.Entry(useritem).State = EntityState.Added;
                result = await CharManager.Context.SaveChangesAsync();

                if (result > 0)
                {
                    // If successful, subtract money from character and save
                    character.Gold -= item.BuyCost;
                    CharManager.Context.Entry(character).State = EntityState.Modified;
                    result = await CharManager.Context.SaveChangesAsync();

                    // If successful, respond with how much the item cost
                    if (result > 0)
                        goldSpent = item.BuyCost.ToString();
                }
            }
            else
            {
                // Setting variable to reflect that inventory was full
                goldSpent = "full";
            }

            return Json(goldSpent);
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

        private string AccountAge(DateTime created)
        {
            var totaldays = DaysBetween(created, DateTime.Now);
            var years = totaldays / 365;
            var days = totaldays % 365;

            return string.Format("{0} Years, {1} Days", years, days);
        }

        private string LastLogin(DateTime lastlogin)
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

        private int DaysBetween(DateTime d1, DateTime d2)
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
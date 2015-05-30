using CrescentIsland.Website.Models.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace CrescentIsland.Website.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationUserManager _userManager;

        public UserRepository()
        {
        }
        public UserRepository(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<bool> UpdateHealth(int healthChange, int maxHealthChange)
        {
            var user = await UserManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            var propertyUpdates = new List<PropertyUpdate>();
            bool modifiedMax = false;

            if (user == null) return false;
            
            if (maxHealthChange > 0)
            {
                user.MaxHealth += maxHealthChange;
                propertyUpdates.Add(PropertyUpdate.MaxHealth);
                modifiedMax = true;
            }

            if (user.CurHealth > 0)
            {
                user.CurHealth += healthChange;
                if (user.CurHealth < 0) user.CurHealth = 0;
                else if (user.CurHealth > user.MaxHealth) user.CurHealth = user.MaxHealth;
                propertyUpdates.Add(PropertyUpdate.CurHealth);
            }

            using (var db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>())
            {
                db.Users.Attach(user);
                db.Entry(user).Property(x => x.CurHealth).IsModified = true;
                if (modifiedMax) db.Entry(user).Property(x => x.MaxHealth).IsModified = true;
                var result = await db.SaveChangesAsync();
            }

            MvcApplication.SetGlobalModel(user, propertyUpdates);

            return true;
        }

        public async Task<bool> UpdateEnergy(int energyChange, int maxEnergyChange)
        {
            var user = await UserManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            var propertyUpdates = new List<PropertyUpdate>();
            bool modifiedMax = false;

            if (user == null) return false;
            
            if (maxEnergyChange > 0)
            {
                user.MaxEnergy += maxEnergyChange;
                propertyUpdates.Add(PropertyUpdate.MaxEnergy);
                modifiedMax = true;
            }

            if (user.CurEnergy > 0)
            {
                user.CurEnergy += energyChange;
                if (user.CurEnergy < 0) user.CurEnergy = 0;
                else if (user.CurEnergy > user.MaxEnergy) user.CurEnergy = user.MaxEnergy;
                propertyUpdates.Add(PropertyUpdate.CurEnergy);
            }

            using (var db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>())
            {
                db.Users.Attach(user);
                db.Entry(user).Property(x => x.CurEnergy).IsModified = true;
                if (modifiedMax) db.Entry(user).Property(x => x.MaxEnergy).IsModified = true;
                var result = await db.SaveChangesAsync();
            }

            MvcApplication.SetGlobalModel(user, propertyUpdates);

            return true;
        }

        public async Task<bool> UpdateUser()
        {
            var user = await UserManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            if (user == null) return false;

            MvcApplication.SetGlobalModel(user);

            return true;
        }
    }
}
using CrescentIsland.Website.Models.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
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

        public User CreateNewUser(RegisterViewModel model, string mapPath)
        {
            var user = new User
            {
                UserGender = model.Gender.HasValue ? model.Gender.Value : UserGender.None,
                UserClass = model.UserClass.HasValue ? model.UserClass.Value : UserClass.Valkyrie,
                UserName = model.Username,
                Email = model.Email,
                Level = 1,
                CurExp = 0,
                MaxExp = 10,
                Gold = 300,
                CurHealth = 20,
                MaxHealth = 20,
                CurEnergy = 10,
                MaxEnergy = 10
            };

            SetUserStats(ref user);

            var file = string.Empty;
            switch (user.UserGender)
            {
                case UserGender.Male:
                    file = Path.Combine(mapPath, "Assets/Images/Avatars/small-face001.png");
                    break;
                case UserGender.Female:
                    file = Path.Combine(mapPath, "Assets/Images/Avatars/small-face011.png");
                    break;
                case UserGender.None:
                    file = Path.Combine(mapPath, "Assets/Images/Avatars/small-face028.png");
                    break;
                default:
                    file = Path.Combine(mapPath, "Assets/Images/Avatars/small-face028.png");
                    break;
            }

            if (!string.IsNullOrEmpty(file))
            {
                FileStream stream = System.IO.File.OpenRead(file);
                var fileBytes = new byte[stream.Length];

                stream.Read(fileBytes, 0, fileBytes.Length);
                stream.Close();

                var ext = Path.GetExtension(stream.Name).Substring(1);

                user.AvatarImage = fileBytes;
                user.AvatarMimeType = ext;
            }

            return user;
        }

        private void SetUserStats(ref User user)
        {
            switch (user.UserClass)
            {
                case UserClass.Valkyrie:
                    user.Attack = 20;
                    user.Defense = 20;
                    user.MagicAttack = 20;
                    user.MagicDefense = 20;
                    user.Accuracy = 15;
                    user.Evasion = 10;
                    break;
                case UserClass.Warrior:
                    user.Attack = 30;
                    user.Defense = 25;
                    user.MagicAttack = 10;
                    user.MagicDefense = 15;
                    user.Accuracy = 15;
                    user.Evasion = 10;
                    break;
                case UserClass.Sorceress:
                    user.Attack = 10;
                    user.Defense = 15;
                    user.MagicAttack = 30;
                    user.MagicDefense = 25;
                    user.Accuracy = 10;
                    user.Evasion = 15;
                    break;
                case UserClass.Rogue:
                    user.Attack = 25;
                    user.Defense = 15;
                    user.MagicAttack = 10;
                    user.MagicDefense = 15;
                    user.Accuracy = 15;
                    user.Evasion = 25;
                    break;
                case UserClass.Engineer:
                    user.Attack = 15;
                    user.Defense = 25;
                    user.MagicAttack = 15;
                    user.MagicDefense = 25;
                    user.Accuracy = 20;
                    user.Evasion = 5;
                    break;
                case UserClass.Samurai:
                    user.Attack = 30;
                    user.Defense = 20;
                    user.MagicAttack = 10;
                    user.MagicDefense = 15;
                    user.Accuracy = 10;
                    user.Evasion = 20;
                    break;
                default:
                    user.Attack = 0;
                    user.Defense = 0;
                    user.MagicAttack = 0;
                    user.MagicDefense = 0;
                    user.Accuracy = 0;
                    user.Evasion = 0;
                    break;
            }
        }

        public async Task<bool> UpdateHealth(int healthChange, int maxHealthChange)
        {
            if (HttpContext.Current == null) return false;

            var user = await UserManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            if (user == null) return false;

            bool modifiedMax = false;

            if (maxHealthChange > 0)
            {
                user.MaxHealth += maxHealthChange;
                modifiedMax = true;
            }

            if (user.CurHealth > 0)
            {
                user.CurHealth += healthChange;
                if (user.CurHealth < 0) user.CurHealth = 0;
                else if (user.CurHealth > user.MaxHealth) user.CurHealth = user.MaxHealth;
            }

            using (var db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>())
            {
                db.Users.Attach(user);
                db.Entry(user).Property(x => x.CurHealth).IsModified = true;
                if (modifiedMax) db.Entry(user).Property(x => x.MaxHealth).IsModified = true;
                var result = await db.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> UpdateEnergy(int energyChange, int maxEnergyChange)
        {
            if (HttpContext.Current == null) return false;

            var user = await UserManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            if (user == null) return false;

            bool modifiedMax = false;

            if (maxEnergyChange > 0)
            {
                user.MaxEnergy += maxEnergyChange;
                modifiedMax = true;
            }

            if (user.CurEnergy > 0)
            {
                user.CurEnergy += energyChange;
                if (user.CurEnergy < 0) user.CurEnergy = 0;
                else if (user.CurEnergy > user.MaxEnergy) user.CurEnergy = user.MaxEnergy;
            }

            using (var db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>())
            {
                db.Users.Attach(user);
                db.Entry(user).Property(x => x.CurEnergy).IsModified = true;
                if (modifiedMax) db.Entry(user).Property(x => x.MaxEnergy).IsModified = true;
                var result = await db.SaveChangesAsync();
            }

            return true;
        }
    }
}
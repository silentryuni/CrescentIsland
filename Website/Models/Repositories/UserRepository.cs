using CrescentIsland.Website.Models.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CrescentIsland.Website.Models.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private ApplicationUserManager _userManager;
        private CharacterManager _charManager;

        public UserRepository()
        {
        }
        public UserRepository(ApplicationUserManager userManager, CharacterManager charManager)
        {
            UserManager = userManager;
            CharManager = charManager;
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
        public CharacterManager CharManager
        {
            get
            {
                return _charManager ?? HttpContext.Current.GetOwinContext().Get<CharacterManager>();
            }
            private set
            {
                _charManager = value;
            }
        }

        public User CreateNewUser(RegisterViewModel model, string mapPath)
        {
            var user = new User
            {
                Created = DateTime.Now,
                LastLogin = DateTime.Now,

                FirstName = "",
                LastName = "",
                Birthday = new DateTime(model.Year, model.Month, model.Day),
                Country = "",

                UserGender = model.Gender.HasValue ? model.Gender.Value : UserGender.None,
                
                UserName = model.Username,
                Email = model.Email,

                ShowAge = false,
                ShowGender = false,
                ShowMoney = false
            };

            var character = new Character
            {
                Id = Guid.NewGuid().ToString(),
                CharacterName = model.Username,
                UserClass = model.UserClass.HasValue ? model.UserClass.Value : UserClass.Valkyrie,
                Level = 1,
                CurExp = 0,
                MaxExp = 10,
                Gold = 5000,
                CurHealth = 20,
                MaxHealth = 20,
                CurEnergy = 10,
                MaxEnergy = 10
            };
            
            SetUserStats(ref character);

            user.Characters.Add(character);

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
                FileStream stream = File.OpenRead(file);
                var fileBytes = new byte[stream.Length];

                stream.Read(fileBytes, 0, fileBytes.Length);
                stream.Close();

                var ext = Path.GetExtension(stream.Name).Substring(1);

                user.AvatarImage = fileBytes;
                user.AvatarMimeType = ext;
            }

            return user;
        }

        private void SetUserStats(ref Character character)
        {
            switch (character.UserClass)
            {
                case UserClass.Valkyrie:
                    character.Attack = 20;
                    character.Defense = 20;
                    character.MagicAttack = 20;
                    character.MagicDefense = 20;
                    character.Accuracy = 15;
                    character.Evasion = 10;
                    break;
                case UserClass.Warrior:
                    character.Attack = 30;
                    character.Defense = 25;
                    character.MagicAttack = 10;
                    character.MagicDefense = 15;
                    character.Accuracy = 15;
                    character.Evasion = 10;
                    break;
                case UserClass.Sorceress:
                    character.Attack = 10;
                    character.Defense = 15;
                    character.MagicAttack = 30;
                    character.MagicDefense = 25;
                    character.Accuracy = 10;
                    character.Evasion = 15;
                    break;
                case UserClass.Rogue:
                    character.Attack = 25;
                    character.Defense = 15;
                    character.MagicAttack = 10;
                    character.MagicDefense = 15;
                    character.Accuracy = 15;
                    character.Evasion = 25;
                    break;
                case UserClass.Engineer:
                    character.Attack = 15;
                    character.Defense = 25;
                    character.MagicAttack = 15;
                    character.MagicDefense = 25;
                    character.Accuracy = 20;
                    character.Evasion = 5;
                    break;
                case UserClass.Samurai:
                    character.Attack = 30;
                    character.Defense = 20;
                    character.MagicAttack = 10;
                    character.MagicDefense = 15;
                    character.Accuracy = 10;
                    character.Evasion = 20;
                    break;
                default:
                    character.Attack = 0;
                    character.Defense = 0;
                    character.MagicAttack = 0;
                    character.MagicDefense = 0;
                    character.Accuracy = 0;
                    character.Evasion = 0;
                    break;
            }
        }

        public async Task<bool> UpdateHealth(int healthChange, int maxHealthChange)
        {
            if (HttpContext.Current == null) return false;

            var user = await UserManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            if (user == null) return false;
            var character = user.Characters.FirstOrDefault();
            if (character == null) return false;

            bool modifiedMax = false;

            if (maxHealthChange > 0)
            {
                character.MaxHealth += maxHealthChange;
                modifiedMax = true;
            }

            if (character.CurHealth > 0)
            {
                character.CurHealth += healthChange;
                if (character.CurHealth < 0) character.CurHealth = 0;
                else if (character.CurHealth > character.MaxHealth) character.CurHealth = character.MaxHealth;
            }

            var items = character.Items;
            CharManager.Context.Entry(character).Property(x => x.CurHealth).IsModified = true;
            if (modifiedMax) CharManager.Context.Entry(character).Property(x => x.MaxHealth).IsModified = true;
            var result = await CharManager.Context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateEnergy(int energyChange, int maxEnergyChange)
        {
            if (HttpContext.Current == null) return false;

            var user = await UserManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            if (user == null) return false;
            var character = user.Characters.FirstOrDefault();
            if (character == null) return false;

            bool modifiedMax = false;

            if (maxEnergyChange > 0)
            {
                character.MaxEnergy += maxEnergyChange;
                modifiedMax = true;
            }

            if (character.CurEnergy > 0)
            {
                character.CurEnergy += energyChange;
                if (character.CurEnergy < 0) character.CurEnergy = 0;
                else if (character.CurEnergy > character.MaxEnergy) character.CurEnergy = character.MaxEnergy;
            }

            var items = character.Items;
            CharManager.Context.Entry(character).Property(x => x.CurEnergy).IsModified = true;
            if (modifiedMax) CharManager.Context.Entry(character).Property(x => x.MaxEnergy).IsModified = true;
            var result = await CharManager.Context.SaveChangesAsync();

            return true;
        }

        #region Disposing

        private bool disposed;

        ~UserRepository()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                }

                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                if (_charManager != null)
                {
                    _charManager.Dispose();
                    _charManager = null;
                }
            }

            disposed = true;
        }

        #endregion
    }
}
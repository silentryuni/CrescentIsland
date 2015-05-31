using System;
using System.Linq;

namespace CrescentIsland.Website.Models
{
    public class GlobalModel
    {
        public GlobalModel()
        {
            CurrentUser = new CurrentUserModel()
            {
                AvatarUrl = "",
                Username = "",
                IsAdmin = false,
                UserClass = UserClass.Valkyrie,
                Level = 0,
                CurExp = 0,
                MaxExp = 0,
                Gold = 0,
                CurHealth = 0,
                MaxHealth = 0,
                CurEnergy = 0,
                MaxEnergy = 0,
                Attack = 0,
                Defense = 0,
                MagicAttack = 0,
                MagicDefense = 0,
                Accuracy = 0,
                Evasion = 0
            };
        }
        public GlobalModel(User user, string adminRoleId)
        {
            CurrentUser = new CurrentUserModel();

            CurrentUser.AvatarUrl = string.Format("data:image/{0};base64,{1}", user.AvatarMimeType, Convert.ToBase64String(user.AvatarImage, 0, user.AvatarImage.Length));
            CurrentUser.Username = user.UserName;
            CurrentUser.IsAdmin = (user.Roles.Any(r => r.RoleId.Equals(adminRoleId, StringComparison.InvariantCultureIgnoreCase)) ? true : false);
            CurrentUser.UserClass = user.UserClass;
            CurrentUser.Level = user.Level;
            CurrentUser.CurExp = user.CurExp;
            CurrentUser.MaxExp = user.MaxExp;
            CurrentUser.Gold = user.Gold;
            CurrentUser.CurHealth = user.CurHealth;
            CurrentUser.MaxHealth = user.MaxHealth;
            CurrentUser.CurEnergy = user.CurEnergy;
            CurrentUser.MaxEnergy = user.MaxEnergy;
            CurrentUser.Attack = user.Attack;
            CurrentUser.Defense = user.Defense;
            CurrentUser.MagicAttack = user.MagicAttack;
            CurrentUser.MagicDefense = user.MagicDefense;
            CurrentUser.Accuracy = user.Accuracy;
            CurrentUser.Evasion = user.Evasion;
        }

        public CurrentUserModel CurrentUser { get; set; }
    }

    public class CurrentUserModel
    {
        public string AvatarUrl { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }

        public UserClass UserClass { get; set; }
        public int Level { get; set; }
        public int CurExp { get; set; }
        public int MaxExp { get; set; }
        public int Gold { get; set; }

        public int CurHealth { get; set; }
        public int MaxHealth { get; set; }
        public int CurEnergy { get; set; }
        public int MaxEnergy { get; set; }

        public int Attack { get; set; }
        public int Defense { get; set; }
        public int MagicAttack { get; set; }
        public int MagicDefense { get; set; }
        public int Accuracy { get; set; }
        public int Evasion { get; set; }
    }
}

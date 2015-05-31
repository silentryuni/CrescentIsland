using CrescentIsland.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CrescentIsland.Website
{
    public class MvcApplication : HttpApplication
    {
        private static GlobalModel _model;
        private static string adminRoleId;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var adminRole = roleManager.FindByName("Administrator");
            adminRoleId = adminRole.Id;
            SetGlobalModel();
        }

        public static void SetGlobalModel()
        {
            var model = new GlobalModel();
            model.CurrentUser = new CurrentUserModel()
            {
                AvatarUrl = "",
                Username = "",
                IsAdmin = false,
                UserClass = UserClass.Valkyrie,
                Level = 0,
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

            _model = model;
        }
        public static void SetGlobalModel(User user)
        {
            if (user == null) return;

            var model = new GlobalModel();
            model.CurrentUser = new CurrentUserModel();
            
            model.CurrentUser.AvatarUrl = string.Format("data:image/{0};base64,{1}", user.AvatarMimeType, Convert.ToBase64String(user.AvatarImage, 0, user.AvatarImage.Length));
            model.CurrentUser.Username = user.UserName;
            model.CurrentUser.IsAdmin = (user.Roles.Any(r => r.RoleId.Equals(adminRoleId, StringComparison.InvariantCultureIgnoreCase)) ? true : false);
            model.CurrentUser.UserClass = user.UserClass;
            model.CurrentUser.Level = user.Level;
            model.CurrentUser.CurHealth = user.CurHealth;
            model.CurrentUser.MaxHealth = user.MaxHealth;
            model.CurrentUser.CurEnergy = user.CurEnergy;
            model.CurrentUser.MaxEnergy = user.MaxEnergy;
            model.CurrentUser.Attack = user.Attack;
            model.CurrentUser.Defense = user.Defense;
            model.CurrentUser.MagicAttack = user.MagicAttack;
            model.CurrentUser.MagicDefense = user.MagicDefense;
            model.CurrentUser.Accuracy = user.Accuracy;
            model.CurrentUser.Evasion = user.Evasion;

            _model = model;
        }

        public static void SetGlobalModel(User user, List<PropertyUpdate> properties)
        {
            foreach (var property in properties)
            {
                switch (property)
                {
                    case PropertyUpdate.Avatar:
                        _model.CurrentUser.AvatarUrl = string.Format("data:image/{0};base64,{1}", user.AvatarMimeType, Convert.ToBase64String(user.AvatarImage, 0, user.AvatarImage.Length));
                        break;
                    case PropertyUpdate.Level:
                        _model.CurrentUser.Level = user.Level;
                        break;
                    case PropertyUpdate.CurHealth:
                        _model.CurrentUser.CurHealth = user.CurHealth;
                        break;
                    case PropertyUpdate.MaxHealth:
                        _model.CurrentUser.MaxHealth = user.MaxHealth;
                        break;
                    case PropertyUpdate.CurEnergy:
                        _model.CurrentUser.CurEnergy = user.CurEnergy;
                        break;
                    case PropertyUpdate.MaxEnergy:
                        _model.CurrentUser.MaxEnergy = user.MaxEnergy;
                        break;
                    case PropertyUpdate.Attack:
                        _model.CurrentUser.Attack = user.Attack;
                        break;
                    case PropertyUpdate.Defense:
                        _model.CurrentUser.Defense = user.Defense;
                        break;
                    case PropertyUpdate.MagicAttack:
                        _model.CurrentUser.MagicAttack = user.MagicAttack;
                        break;
                    case PropertyUpdate.MagicDefense:
                        _model.CurrentUser.MagicDefense = user.MagicDefense;
                        break;
                    case PropertyUpdate.Accuracy:
                        _model.CurrentUser.Accuracy = user.Accuracy;
                        break;
                    case PropertyUpdate.Evasion:
                        _model.CurrentUser.Evasion = user.Evasion;
                        break;
                    default:
                        break;
                }
            }
        }

        public static GlobalModel GetGlobalModel()
        {
            if ((_model == null ||  _model.CurrentUser == null || _model.CurrentUser.Username == "") && HttpContext.Current.Request.IsAuthenticated)
            {
                var UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = UserManager.FindById(HttpContext.Current.User.Identity.GetUserId());

                if (user == null) return _model;

                SetGlobalModel(user);
            }

            return _model;
        }
    }

    public enum PropertyUpdate
    {
        Avatar,
        Level,
        CurHealth,
        MaxHealth,
        CurEnergy,
        MaxEnergy,
        Attack,
        Defense,
        MagicAttack,
        MagicDefense,
        Accuracy,
        Evasion
    }
}

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
            _model = new GlobalModel();
        }
        public static void SetGlobalModel(User user)
        {
            if (user == null) return;
            
            _model = new GlobalModel(user, adminRoleId);
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
                    case PropertyUpdate.CurExp:
                        _model.CurrentUser.CurExp = user.CurExp;
                        break;
                    case PropertyUpdate.MaxExp:
                        _model.CurrentUser.MaxExp = user.MaxExp;
                        break;
                    case PropertyUpdate.Gold:
                        _model.CurrentUser.Gold = user.Gold;
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
        CurExp,
        MaxExp,
        Gold,
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

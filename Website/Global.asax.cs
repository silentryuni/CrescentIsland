using CrescentIsland.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CrescentIsland.Website
{
    public class MvcApplication : HttpApplication
    {
        private static GlobalModel _model;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SetGlobalModel();
        }

        public static void SetGlobalModel()
        {
            var model = new GlobalModel();
            model.CurrentUser = new CurrentUserModel()
            {
                AvatarUrl = "",
                Username = "",
                Level = "",
                CurHealth = "",
                MaxHealth = "",
                CurEnergy = "",
                MaxEnergy = ""
            };

            _model = model;
        }
        public static void SetGlobalModel(User user)
        {
            if (user == null) return;

            var model = new GlobalModel();
            model.CurrentUser = new CurrentUserModel();

            model.CurrentUser.AvatarUrl = "/Assets/Images/tmp-avatar.png";
            model.CurrentUser.Username = user.UserName;
            model.CurrentUser.IsAdmin = (user.UserName == "Ryuni" ? true : false);
            model.CurrentUser.Level = (user.UserName == "Ryuni" ? "100" : "5");
            model.CurrentUser.CurHealth = "18";
            model.CurrentUser.MaxHealth = "25";
            model.CurrentUser.CurEnergy = "6";
            model.CurrentUser.MaxEnergy = "10";

            _model = model;
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
}

using System.Web.Mvc;
using System.Web.Routing;

namespace CrescentIsland.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "CharacterInventory",
                url: "Character/Inventory",
                defaults: new { controller = "Character", action = "Inventory" }
            );

            routes.MapRoute(
                name: "Character",
                url: "Character/{charname}",
                defaults: new { controller = "Character", action = "Index", charname = "" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Page", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

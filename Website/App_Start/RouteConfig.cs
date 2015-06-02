﻿using System.Web.Mvc;
using System.Web.Routing;

namespace CrescentIsland.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Character",
                url: "Character/{username}",
                defaults: new { controller = "Character", action = "Index", username = "" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Page", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

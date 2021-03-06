﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Zilla
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapMvcAttributeRoutes();


            routes.MapRoute(
                name: "TeamMemberRemoval",
                url: "Teams/RemoveMember/{id}/{memberIndex}",
                defaults: new { controller = "Teams", action = "RemoveMember" }
            );

            #region Tweaks 
            /*
            routes.MapRoute(
                "CreateRoute",            
                "{controller}/Create",    
                new { action = "Create" } 
            );
            routes.MapRoute(
                "DetailsRoute",           
                "{controller}/{id}",      
                new { action = "Details" }
            );
            */
            routes.MapRoute(
                name: "AboutFaraCasa",
                url: "About",
                defaults: new { controller = "Home", action = "About" }
            );
            routes.MapRoute(
                name: "ContactFaraCasa",
                url: "Contact",
                defaults: new { controller = "Home", action = "Contact" }
            );
            routes.MapRoute(
                name: "Index",
                url: "{controller}",
                defaults: new { action = "Index" }
            );

            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

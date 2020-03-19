using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Volunteers
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("JobDetails", "j/{id}", new
            {
                controller = "Jobs",
                action = "Details",
                id = UrlParameter.Optional
            });

            routes.MapRoute("Login",
               "login",
               new { controller = "Account", action = "Login" });

            routes.MapRoute("Forgot",
               "forgot",
               new { controller = "Account", action = "ForgotPassword" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

           
        }
    }
}

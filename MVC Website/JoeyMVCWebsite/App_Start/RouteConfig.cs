using System.Web.Mvc;
using System.Web.Routing;

namespace JoeyMVCWebsite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //{Admin} is a polaceholder, the 1st one,which means if url entered inclueds "Admin:, ppage will beredirected to knowledge, however the second route will never get used. in order to fixthis unexpected issue, we should swithcthe order of two routes

            //Tthe one belwo must be existing, cause route class needs a default route
            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Knowledge", action = "Index", id = UrlParameter.Optional }
        );

            routes.MapRoute(
                name: "admin",
                url: "Admin",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Test",
            url: "{Admin}",
            defaults: new { controller = "Knowledge", action = "Knowledge", id = UrlParameter.Optional }
            );
        }
    }
}
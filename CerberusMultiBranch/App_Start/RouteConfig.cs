using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CerberusMultiBranch
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "PurchaseEstimation",
              url: "Purchasing/PurchaseEstimation",
              defaults: new { controller = "PurchaseEstimation", action = "PurchaseEstimation", id = UrlParameter.Optional });

         
            routes.MapRoute(
             name: "Clients",
             url: "Catalog/Clients",
             defaults: new { controller = "Clients", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "MakesAndModels",
                url: "Catalog/MakesAndModels",
                defaults: new { controller = "MakesAndModels", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
            name: "Clasifications",
            url: "Catalog/Clasifications",
            defaults: new { controller = "Configuration", action = "Clasifications", id = UrlParameter.Optional });

            routes.MapRoute(
             name: "Employees",
             url: "Catalog/Employees",
             defaults: new { controller = "Employees", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
             name: "Providers",
             url: "Catalog/Suppliers",
             defaults: new { controller = "Providers", action = "Index", id = UrlParameter.Optional });



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "CashRegister", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

using System.Web.Mvc;
using System.Web.Routing;

namespace Template
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            "ContentHash",
            "c/{hash}",
            new { controller = "ContentFile", action = "Get", area = "" },
            new { hash = @"^[0-9a-zA-Z]+$" } // constraint
            );

            #region Email

            routes.MapRoute(
                name: "EmailWatcher",
                url: "Analytics/{EmailAuditGuidId}",
                defaults: new { controller = "Email", action = "EmailWatcher", EmailAuditGuidId = UrlParameter.Optional },
                namespaces: new[] { "Website.Controllers" }
                );


            #endregion

            #region Search



            routes.MapRoute(
                name: "SearchAutocomplete",
                url: "Search/GetSearchAutocomplete/{term}",
                defaults: new { controller = "Search", action = "GetSearchAutocomplete", term = UrlParameter.Optional },
                namespaces: new[] { "Website.Controllers" }
                );

            routes.MapRoute(
                name: "Search",
                url: "SearchItems/{pattern}",
                defaults: new { controller = "Search", action = "Index", pattern = UrlParameter.Optional },
                namespaces: new[] { "Website.Controllers" }
                );

            routes.MapRoute(
                name: "SearchUsers",
                url: "SearchUsers",
                defaults: new { controller = "Search", action = "SearchUsers" },
                namespaces: new[] { "Website.Controllers" }
                );


            #endregion

            #region Error

            routes.MapRoute(
                "Error404",
                "Error404",
                new { controller = "Error", action = "Error404" },
                namespaces: new[] { "Website.Controllers" }
            );



            #endregion

            #region Home

            routes.MapRoute(
                "ContactUs",
                "ContactUs",
                new { controller = "Home", action = "ContactUs" },
                namespaces: new[] { "Website.Controllers" }
            );

            routes.MapRoute(
                "FAQ",
                "FAQ",
                new { controller = "Home", action = "FAQ" },
                namespaces: new[] { "Website.Controllers" }
            );

            #endregion

            #region Account



            routes.MapRoute(
                "PasswordChanged",
                "PasswordChanged",
                new { controller = "Account", action = "PasswordChanged" },
                namespaces: new[] { "Website.Controllers" }
            );

            routes.MapRoute(
                "ResetPassword",
                "ResetPassword/{UserId}/{Token}",
                new { controller = "Account", action = "ResetPassword", UserId = UrlParameter.Optional, Token = UrlParameter.Optional },
                namespaces: new[] { "Website.Controllers" }
            );

       

           routes.MapRoute(
                "ConfirmEmail",
                "ConfirmEmail/{UserId}/{Token}",
                new { controller = "Account", action = "ConfirmEmail", UserId = UrlParameter.Optional, Token = UrlParameter.Optional },
                namespaces: new[] { "Website.Controllers" }
            );

            routes.MapRoute(
                "TermsAndConditions",
                "TermsAndConditions",
                new { controller = "Account", action = "TermsAndConditions" },
                namespaces: new[] { "Website.Controllers" }
            );

            routes.MapRoute(
                "PrivacyPolicy",
                "PrivacyPolicy",
                new { controller = "Account", action = "PrivacyPolicy" },
                namespaces: new[] { "Website.Controllers" }
            );

            routes.MapRoute(
                "Login",
                "Login",
                new { controller = "Account", action = "Login" },
                namespaces: new[] { "Website.Controllers" }
            );




            routes.MapRoute(
                name: "MyOtherProfile",
                url: "MyProfile/{id}/{show}",
                defaults: new { controller = "Profile", action = "Index", show = UrlParameter.Optional, id = UrlParameter.Optional },
                namespaces: new[] { "Website.Controllers" },
                constraints: new { id = @"\d+" }
                );

            routes.MapRoute(
                name: "MyProfile",
                url: "MyProfile/{show}",
                defaults: new { controller = "Profile", action = "Index", show = UrlParameter.Optional },
                namespaces: new[] { "Website.Controllers" }
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

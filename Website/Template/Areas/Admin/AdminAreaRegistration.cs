using System.Web.Mvc;

namespace Website.Areas.Admin.Controllers
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "Admin_home",
                "Admin/",
                new { controller = "Users", action = "Index" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 new[] { "Website.Areas.Admin.Controllers" }
            );
        }
    }
}
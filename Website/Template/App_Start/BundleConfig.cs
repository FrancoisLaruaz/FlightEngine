using System.Web.Mvc;
using System.Web.Routing;
using System;
using System.Web.Optimization;
using Commons;

namespace Template
{
    public class BundleConfig
    {
        public static void RegisterJSBundles(BundleCollection bundles)
        {
            try
            {
                bundles.Add(new ScriptBundle("~/Scripts/JQuery").Include(
                    "~/Scripts/jquery-2.2.3.min.js",
                    "~/Scripts/jquery-ui.min.js"
                    ));

                bundles.Add(new ScriptBundle("~/Scripts/JQueryVal").Include(
                            "~/Scripts/jquery.validate.min.js",
                            "~/Scripts/jquery.validate.unobtrusive.min.js",
                            "~/Scripts/jquery.unobtrusive-ajax.min.js"));
                /*
                bundles.Add(new ScriptBundle("~/Scripts/JQuery").Include(
          "~/Scripts/jquery-{version}.js",
           "~/Scripts/jquery-ui.min.js"
           ));

                bundles.Add(new ScriptBundle("~/Scripts/JQueryVal").Include(
                            "~/Scripts/jquery.validate*",
                            "~/Scripts/jquery.unobtrusive*"));
                            */
                bundles.Add(new ScriptBundle("~/Scripts/ScriptsErrors").Include(
                "~/Scripts/General/BrowserHelper.js",
                "~/Scripts/General/Toastr.js",
                "~/Scripts/General/ToastrHelper.js",
                "~/Scripts/General/Utils.js",
                "~/Scripts/General/Error.js",
                 "~/Scripts/General/Cookie.js",
                "~/Scripts/General/CookieHelper.js",
                 "~/Scripts/General/LanguageHelper.js"
                ));


                bundles.Add(new ScriptBundle("~/Scripts/Scripts").Include(
                "~/Scripts/jquery.webcam.js",
                "~/Scripts/General/CarouselHelper.js",
                 "~/Scripts/Views/Shared/_CookieConsent.js",
                 "~/Scripts/Views/Shared/_TawkToHelper.js",
                 "~/Scripts/General/DateHelper.js",
                  "~/Scripts/General/FormaterHelper.js",
                "~/Scripts/General/CaptchaHelper.js",
                "~/Scripts/General/Responsive.js",
                 "~/Scripts/moment.min.js",
                 "~/Scripts/bootstrap-datetimepicker.min.js",
                 "~/Scripts/General/daterangepicker.js",
                 "~/Scripts/General/BackToTop.js",
                "~/Scripts/General/AudioHelper.js",
                "~/Scripts/General/FileFormHelper.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/General/bootstrap-datepicker.js",
                "~/Scripts/General/bootstrap-slider.js",
                "~/Scripts/General/jtsage-datebox.min.js",
                "~/Scripts/General/jtsage-doc.js",
                "~/Scripts/General/SweetAlert.js",
                "~/Scripts/General/respond.js",
                "~/Scripts/General/DateCompare.js",
                "~/Scripts/General/CustomValidation.js",
                "~/Scripts/General/modernizr.js",
                "~/Scripts/General/SweetAlertHelper.js",
                "~/Scripts/General/FileDrop.js",
                "~/Scripts/General/FileDropHelper.js",
                 "~/Scripts/General/FlashVersion.js",
                "~/Scripts/General/WebcamHelper.js",
                 "~/Scripts/General/PopUp.js",
                "~/Scripts/Views/Account/_PasswordPolicy.js",
                "~/Scripts/General/lightbox.min.js"
              // "~/Scripts/General/Highcharts.js",
              //  "~/Scripts/General/HighchartsExporting.js",
              //  "~/Scripts/General/HighchartsHelper.js"
              , "~/Scripts/General/summernote.js"
               , "~/Scripts/General/summernoteHelper.js"
                , "~/Scripts/General/SexyCSSHelper.js"
                , "~/Scripts/General/Main.js"

                ));
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

        }

        public static void RegisterCSSBundles(BundleCollection bundles)
        {
            try
            {

                bundles.Add(new LessBundle("~/bundles/Style").Include("~/Content/bootstrap.min.css", new CssRewriteUrlTransform()).Include("~/Content/font-awesome.min.css", new CssRewriteUrlTransform()).Include(
                     "~/Content/General/bootstrap-slider.css",
                    "~/Content/General/bootstrap-datepicker.css",
                    "~/Content/General/bootstrap-social.css",
                    "~/Content/General/animate.css",
                    "~/Content/General/_Spinner.css",
                    "~/Content/General/Carousel.css",
                     "~/Content/Views/Shared/_CookieConsent.css",
                     "~/Content/General/lightbox.min.css",
                    "~/Content/General/DragAndDrop.css",
                    "~/Content/General/sweetalert.css",
                    "~/Content/General/PopUp.css",
                    "~/Content/jquery-ui-min.css",
                     "~/Content/General/fakeSocialMediaStyle.css",
                    "~/Content/General/toastr.csss",
                   "~/Content/General/webcam.css",
                   "~/Content/General/googleMap.css",
                   "~/Content/Views/Shared/_LanguageSelector.css",
                    "~/Content/General/jtsage-datebox.min.css",
                    "~/Content/General/jtsage-syntax.css",
                    "~/Content/Views/Account/_PasswordPolicy.css",
                    "~/Content/General/main.less"
                )); 

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            try
            {
                RegisterCSSBundles(bundles);
                RegisterJSBundles(bundles);

                //this forces development mode to also minify and compress scripts bundle
                if (Utils.IsProductionWebsite())
                {
                    BundleTable.EnableOptimizations = true;
                }
                else
                {
                    BundleTable.EnableOptimizations = false;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}

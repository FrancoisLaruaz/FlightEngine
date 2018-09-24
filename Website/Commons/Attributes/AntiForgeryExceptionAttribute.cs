using Models.Class;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Commons.Encrypt;

namespace Commons.Attributes
{
    public class AntiForgeryExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            try
            {
                // ANTIFORGERY TOKEN NOT PRESENT
                if (!filterContext.ExceptionHandled && filterContext.Exception is HttpAntiForgeryException)
                {
                    var request = new HttpRequestWrapper(System.Web.HttpContext.Current.Request);
                    string parameters = "";
                    if (request != null)
                    {
                        // Use your own logging service to log the results
                        parameters = "IP = " + filterContext.RequestContext.HttpContext.Request.UserHostAddress;
                        foreach (var key in request.Form.AllKeys)
                        {
                            var value = request.Form[key];
                            if (key.ToLower().Contains("password"))
                            {
                                value = EncryptHelper.EncryptToString(value);
                            }
                            if (!String.IsNullOrWhiteSpace(parameters))
                            {
                                parameters = parameters + " & ";
                            }
                            parameters = parameters + key + " = " + value;
                        }
                    }
                    Logger.GenerateInfo("HttpAntiForgery Token missing : parameters => " + parameters);


                    filterContext.ExceptionHandled = true;

                    //var LoginRedirect = ConfigurationManager.AppSettings["WebsiteURL"] + "/Account/AutomaticLogOffAndRedirectToLogin";
                    // filterContext.Result = new RedirectResult(LoginRedirect);
                    return;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}

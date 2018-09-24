using Models.Class;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Commons.Encrypt;
using System.Web.Routing;

namespace Commons.Attributes
{
    public class AntiForgeryExceptionAttributeLogOff : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            try
            {
                // ANTIFORGERY TOKEN NOT PRESENT
                if (!filterContext.ExceptionHandled && filterContext.Exception is HttpAntiForgeryException)
                {
                    var request = new HttpRequestWrapper(System.Web.HttpContext.Current.Request);
                    // Use your own logging service to log the results
                    string parameters = "IP = " + filterContext.RequestContext.HttpContext.Request.UserHostAddress;
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

                    filterContext.ExceptionHandled = true;

                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                                        { "controller", "Home" },
                                        { "action", "Index" }
                            });
                }

                return;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}

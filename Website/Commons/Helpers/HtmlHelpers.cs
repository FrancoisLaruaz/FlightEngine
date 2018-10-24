using Models.Class;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Configuration;
using CommonsConst;
using Models.Class.FileUpload;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Routing;
using System.Web.Mvc;

namespace Commons
{

    public static class HtmlHelpers
    {


        public static RouteValueDictionary ConditionalDisable(bool disabled, object htmlAttributes = null)
        {
            try
            {
                var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                if (disabled)
                    dictionary.Add("disabled", "disabled");
                return dictionary;
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,string.Format("disabled: ", disabled));
            }
            return null;
        }
    }
}

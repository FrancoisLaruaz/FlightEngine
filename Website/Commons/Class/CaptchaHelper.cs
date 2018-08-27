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

namespace Commons
{

    public static class CaptchaHelper
    {
        private static string PublicKey = ConfigurationManager.AppSettings["CaptchaPublicKey"];
        private static string SecretKey = ConfigurationManager.AppSettings["CaptchaSecretKey"];

        public static bool CheckCaptcha(string response)
        {
            bool result = false;
            try
            {
                // ... validate null or empty value if you want
                // then
                // make a request to recaptcha api
                using (var wc = new WebClient())
                {
                    var validateString = string.Format(
                        "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                       SecretKey,    // secret recaptcha key
                       response); // recaptcha value
                                  // Get result of recaptcha
                    var recaptcha_result = wc.DownloadString(validateString);
                    // Just check if request make by user or bot
                    if (recaptcha_result == null || recaptcha_result.ToLower().Contains("false"))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                // Do your work if request send from human :)
            }
            catch (Exception e)
            {
                result = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "response = " + response);
            }
            return result;

        }
    }
}

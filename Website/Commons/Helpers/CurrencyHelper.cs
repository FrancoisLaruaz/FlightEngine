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
using System.Web.Script.Serialization;

namespace Commons
{

    public static class CurrencyHelper
    {


        public static decimal GetDefaultCurrencyConversionRate(string CurrencyCode)
        {
            decimal result = -1;
            try
            {
   
                using (var wc = new WebClient())
                {
                    var validateString = string.Format(
                        "http://free.currencyconverterapi.com/api/v5/convert?q={0}_{1}&compact=y",
                       CurrencyCode,CommonsConst.Const.DefaultCurrency); 
               
                    string request_result = wc.DownloadString(validateString);
                    if (request_result != null && request_result != "")
                    {
                        JavaScriptSerializer srRequestResult = new JavaScriptSerializer();
                        dynamic jsondataRequestResult = srRequestResult.DeserializeObject(request_result);
                        if(jsondataRequestResult!=null)
                        {
                            if (Utils.IsPropertyExist(jsondataRequestResult, CurrencyCode + "_" + CommonsConst.Const.DefaultCurrency))
                            {
                                var value = jsondataRequestResult[CurrencyCode + "_" + CommonsConst.Const.DefaultCurrency];
                                result = Convert.ToDecimal(value["val"]);
                            }
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                result = -1;
                if (!e.Message.Contains("403"))
                {
                    Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CurrencyCode = " + CurrencyCode);
                }
            }
            return result;

        }
    }
}

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
using System.Web.Hosting;
using RestSharp;

namespace Commons
{
    public class DateHelper
    {
        public static DateTime? GetDateFromunixTimeStamp(string unixTimeStamp)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(unixTimeStamp))
                {
                    double unixTime = Convert.ToDouble(unixTimeStamp);

                    long timeInTicks = (long)unixTime;
                    return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timeInTicks);

                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "unixTimeStamp = " + unixTimeStamp);
            }
            return null;
        }


        public static string GetUnixTimeStamp(DateTime? date)
        {
            try
            {
                if (date != null)
                {
                    DateTime midnightDate = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day);


                    long unixTimestamp = (long)(midnightDate -
                             new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;

                    return unixTimestamp.ToString();
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "date = " + date);
            }
            return null;
        }
    }
}

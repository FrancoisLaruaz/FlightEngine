using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;

namespace FlightsEngine.Utils
{
    public static class Utils
    {
        public static bool IsPropertyExist(dynamic settings, string name)
        {
            bool result = false;
            try
            {
                result = ((IDictionary<string, object>)settings).ContainsKey(name);
            }
            catch (Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "name = " + name);
            }
            return result;
        }

        public static DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            double unixTime = Convert.ToDouble(unixTimeStamp);
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTime);
            return dtDateTime;
        }

        public static DateTime? GetDateFromunixTimeStamp(string unixTimeStamp)
        {
            try
            {
                // https://www.unixtimestamp.com/index.php
                if (!String.IsNullOrWhiteSpace(unixTimeStamp))
                {
                    double unixTime = Convert.ToDouble(unixTimeStamp);
                    // Unix timestamp is seconds past epoch
                    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dtDateTime = dtDateTime.AddSeconds(unixTime);
                    return dtDateTime;

                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "unixTimeStamp = " + unixTimeStamp);
            }
            return null;
        }
    }
}

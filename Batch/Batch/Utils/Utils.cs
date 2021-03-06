﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using Newtonsoft.Json;

namespace FlightsEngine.Utils
{
    public static class Utils
    {
        public static bool IsObjectsList(dynamic settings)
        {
            bool result = false;
            try
            {
                var castAttempt = (List<object>)settings;
                result = true;
            }
            catch (Exception e)
            {
                result = false;
               // FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        public static bool IsObjectsArray(dynamic settings)
        {
            bool result = false;
            try
            {
                var castAttempt = (object[])settings;
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                // FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

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
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "name = " + name+" abd json = "+ ( JsonConvert.SerializeObject(settings) ??""));
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

        public static int GetDaysAtDestination(DateTime dateFrom,DateTime dateTo)
        {
            int result = 0;
            try
            {
                DateTime dateFromFormat = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day);
                DateTime dateToFormat = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day); 
   
                if (dateFromFormat < dateToFormat)
                {
                    result = Convert.ToInt32((dateToFormat - dateFromFormat).TotalDays);
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "dateFrom = " + dateFrom+ " and dateTo  ="+ dateTo);
            }
            return result;
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

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace Commons
{
    public static class Utils
    {

        public static string Website = System.Configuration.ConfigurationManager.AppSettings["Website"].ToString();

        public static bool IsProductionWebsite()
        {
            bool result = true;
            try
            {
                if (Website.Contains("localhost:"))
                    result = false;
            }
            catch (Exception e)
            {
                result= true;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Indicate if a property exists
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool DoesPropertyExist(dynamic settings, string name)
        {
            bool result = false;
            try
            {
                result = ((IDictionary<string, object>)settings).ContainsKey(name);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "name ="+ name);
            }
            return result;
        }

        /// <summary>
        /// Generate an error
        /// </summary>
        public static void GenerateError()
        {
            int n = 0;
            n = 5 / n;
        }


        public static bool IsLocalhost()
        {
            bool result = false;
            try
            {
                if (Website.Contains("localhost:"))
                    result = true;
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }



        /// <summary>
        /// Get the list of all the properties
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<object> GetPropertiesList(System.Type type)
        {
            List<object> Result = new List<object>();
            try
            {
                FieldInfo[] fieldInfos = type.GetFields(
                // Gets all public and static fields

                BindingFlags.Public | BindingFlags.Static |
                // This tells it to get the fields from all base types as well

                BindingFlags.FlattenHierarchy);

                // Go through the list and only pick out the constants
                foreach (FieldInfo fi in fieldInfos)
                {
                    // IsLiteral determines if its value is written at 
                    //   compile time and not changeable
                    // IsInitOnly determine if the field can be set 
                    //   in the body of the constructor
                    // for C# a field which is readonly keyword would have both true 
                    //   but a const field would have only IsLiteral equal to true
                    if (fi.IsLiteral && !fi.IsInitOnly)
                        Result.Add(fi.GetValue(null));
                }
                return Result;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "type = "+ type.ToString());
            }
            // Return an array of FieldInfos
            return null;
        }

        /// <summary>
        /// Get the url of a the website
        /// </summary>
        /// <param name="Langtag"></param>
        /// <returns></returns>
        public static string GetURLWebsite(string Langtag=null)
        {
            string result = "";
            try
            {
                string authority = HttpContext.Current.Request.Url.Authority;
                string scheme= HttpContext.Current.Request.Url.Scheme;
                result = scheme +"://"+ authority;

                if(Langtag!=null)
                {
                    result = result + "/" + Langtag;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Remove the unsafe characters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveUnsafeCharacters(string value)
        {
            if (value == null)
                return null;
            else
                return value.Replace("<", "").Replace(">", "").Replace(":", "").Replace("\"", "").Replace("/", "").Replace("\\", "").Replace("|", "").Replace("?", "").Replace("*", "");
        }

        /// <summary>
        /// Check if the string is a valid email
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsValidMail(string address)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(address))
                    return false;

                EmailAddressAttribute e = new EmailAddressAttribute();
                if (e.IsValid(address))
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "address = "+ address);
            }
            return false;
        }

        /// <summary>
        /// Get a random password
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateRandomPassword(int length=10)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }




    }
}

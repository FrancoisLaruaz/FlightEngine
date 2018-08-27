using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;

namespace Commons
{
    public static class MySQLHelper
    {
        // '2017-01-20 23:00:00'
        /// <summary>
        /// Renvoie un string avec le format my sql
        /// </summary>
        /// <returns>The format string pour date my sql.</returns>
        /// <param name="date">Date.</param>
        public static string GetFormatStringForDate(DateTime? date)
        {
            string result = "1990-01-01 23:00:00";
            try
            {
                if (date != null)
                {
                    result = date.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "date = " + date);
            }
            return result;
        }

        public static bool? GetBoolFromMySQL(Object Ob)
        {
            bool? result = null;
            try
            {
                if (Ob != DBNull.Value && Ob != null)
                {
                    result = Convert.ToBoolean(Ob);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Ob = " + Ob.ToString());
            }
            return result;
        }

        public static string GetStringFromMySQL(Object Ob)
        {
            string result = null;
            try
            {
                if (Ob != DBNull.Value && Ob != null)
                {
                    result = Convert.ToString(Ob);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Ob = " + Ob.ToString());
            }
            return result;
        }

        public static int? GetIntFromMySQL(Object Ob)
        {
            int? result = null;
            try
            {
                if (Ob != DBNull.Value && Ob != null)
                {
                    result = Convert.ToInt32(Ob);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Ob = " + Ob.ToString());
            }
            return result;
        }

        public static DateTime? GetDateFromMySQL(Object Ob)
        {
            DateTime? result = null;
            try
            {
                if (Ob != DBNull.Value && Ob != null)
                {
                    result = Convert.ToDateTime(Ob);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Ob = " + Ob.ToString());
            }
            return result;
        }


        /// <summary>
        /// Function used to update/insert any object with mysql
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetValueToInsert(Object Value)
        {
            string result = "NULL";
            try
            {
                if (Value != null)
                {

                    Type ValueType = Value.GetType();

                    if (ValueType == typeof(DateTime))
                    {
                        result = MySQLHelper.GetDateTimeToInsert(Convert.ToDateTime(Value));
                    }
                    else if (ValueType == typeof(Int32) || ValueType == typeof(Int16) || ValueType == typeof(Int64))
                    {
                        result = MySQLHelper.GetIntToInsert(Convert.ToInt32(Value));
                    }
                    else if (ValueType == typeof(Boolean))
                    {
                        result = MySQLHelper.GetBoolToInsert(Convert.ToBoolean(Value));
                    }
                    else if (ValueType == typeof(Decimal))
                    {
                        result = MySQLHelper.GetDecimalToInsert(Convert.ToDecimal(Value));
                    }
                    else
                    {
                        result = MySQLHelper.GetStringToInsert(Convert.ToString(Value));
                    }


                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Value = " + Value);
            }
            return result;
        }


        /// <summary>
        /// Function used to update/insert a string with mysql
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetStringToInsert(string Value)
        {
            string result = "NULL";
            try
            {
                if (Value != null)
                {
                    result = "'" + Value.Replace("'", "''") + "'";
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Value = " + Value);
            }
            return result;
        }

        /// <summary>
        /// Function used to update/insert an integer with mysql
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetIntToInsert(int? Value)
        {
            string result = "NULL";
            try
            {
                if (Value != null)
                {
                    result = Value.ToString();
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Value = " + Value);
            }
            return result;
        }


        /// <summary>
        /// Function used to update/insert a decimal with mysql
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetDecimalToInsert(decimal? Value)
        {
            string result = "NULL";
            try
            {
                if (Value != null)
                {
                    result = Value.ToString();
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Value = " + Value);
            }
            return result;
        }


        /// <summary>
        /// Function used to update/insert a aboolean with mysql
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetBoolToInsert(bool? Value)
        {
            string result = "NULL";
            try
            {
                if (Value != null)
                {
                    if (Value.Value)
                    {
                        result = "1";
                    }
                    else
                    {
                        result = "0";
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Value = " + Value);
            }
            return result;
        }

  

        /// <summary>
        /// Function used to update/insert a date with mysql
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetDateTimeToInsert(DateTime? Value)
        {
            string result = "NULL";
            try
            {
                if (Value != null)
                {
                    result = "'" + GetFormatStringForDate(Value) + "'";
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Value = " + Value);
            }
            return result;
        }
    }
}

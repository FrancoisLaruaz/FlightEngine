using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;
using CommonsConst;

namespace Commons
{ 
    public static class FormaterHelper
    {
 
        /// <summary>
        /// Gets the format string pour date affichage.
        /// </summary>
        /// <returns>The format string pour date affichage.</returns>
        /// <param name="date">Date.</param>
        public static string GetFormatStringForDateDisplay(DateTime? date)
        {
            string result = "";

            if (date != null)
            {
                result = date.Value.ToString(TimeFormat.Month_Day_Year_Time_12h);
            }
            return result;
        }

        /// <summary>
        /// Créations de liens pour les adresses url
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UrlReformate(string text)
        {
            return Regex.Replace(
               text,
                "((?:http|https|ftp)(?::\\/{2}[\\w]+)(?:[\\/|\\.]?)(?:[^\\s\"]*))",
                "<a style='color:black;text-decoration: underline;' href=\"$0\">$0</a>",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        /// Formats the number space separator.
        /// </summary>
        /// <returns>The number space separator.</returns>
        /// <param name="number">Number.</param>
        public static string FormatNumberSpaceSeparator(int? number, string strCulture = null)
        {
            string result = "";
            try
            {
                if (number == null)
                    return "";
                if (String.IsNullOrEmpty(strCulture))
                    strCulture = CommonsConst.Const.DefaultCulture;
                CultureInfo Culture = CultureInfo.CreateSpecificCulture(strCulture);

                string numbercullture = number.Value.ToString("N", Culture);
                result = numbercullture.Split(',')[0];
            }
            catch (Exception e)
            {
                result = "";
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "number = " + number);
            }

            return result;
        }
    }
}

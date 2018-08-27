using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Extensions
{
    public static class StringExtension
    {
        public static string SpaceBeforeCapitals(this string inputString)
        {
            string result = null;
            try
            {
                var sb = new StringBuilder();
                var count = 0;

                foreach (var c in inputString.ToArray())
                {
                    if ((count - 1 > 0) && (char.IsUpper(c) && !char.IsUpper(inputString[count - 1])))
                    {
                        sb.Append(string.Format(" {0}", c));
                    }
                    else if (((count - 2) > 0) && char.IsUpper(c) && char.IsUpper(inputString[count - 1]) && char.IsUpper(inputString[count - 2]))
                    {
                        sb.Append(string.Format(" {0}", c));
                    }
                    else
                    {
                        sb.Append(c);
                    }

                    count++;
                }
                result = sb.ToString();
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "inputString = " + inputString);
            }
            return result;
        }
    }
}

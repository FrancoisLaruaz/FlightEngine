using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Extensions
{
    public static class DateTimeExtension
    {
        public static int? GetAge(this DateTime now, DateTime? birthDate)
        {
            int? age = null;
            try
            {
                if (birthDate==null || now < birthDate)
                {
                    age=null;
                }
                else
                {
                    age = now.Year - birthDate.Value.Year;
                    if (birthDate.Value.Date > now.AddYears(-age.Value).Date)
                    {
                        age--;
                    }
                }
            }
            catch(Exception e)
            {
                string strBirthDate = "NULL";
                if (birthDate!=null)
                {
                    strBirthDate = birthDate.ToString();
                }
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "birthDate = " + strBirthDate);
            }
            return age;
        }
    }
}

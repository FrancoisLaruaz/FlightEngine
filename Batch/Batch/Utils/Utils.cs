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
    }
}

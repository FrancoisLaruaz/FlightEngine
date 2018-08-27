using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommonsConst
{
    public static class ConstantsHelper
    {
        public static Dictionary<string, Dictionary<string, object>> GetJSonConstants()
        {
            Dictionary<string, Dictionary<string, object>> result = new Dictionary<string, Dictionary<string, object>>();
            try
            {

                var ClassesList = from t in Assembly.GetExecutingAssembly().GetTypes().
                        Where(t => t.FullName.Contains("CommonsConst") && !t.FullName.Contains("ConstantsHelper")  && t.BaseType.Name.ToLower() != "enum" && !t.BaseType.Name.ToLower().Contains("runtimemodule"))
                                  select t;
                foreach (var elem in ClassesList.ToList())
                {
                    string name = elem.Name;
                    var constants = elem.GetFields().ToDictionary(x => x.Name, x => x.GetValue(null));
                    if (constants!=null && constants.Count > 0)
                    {
                        result.Add(name, constants);
                    }
                }
            }
            catch (Exception e)
            {
                
            }
            return result;
        }
    }

}
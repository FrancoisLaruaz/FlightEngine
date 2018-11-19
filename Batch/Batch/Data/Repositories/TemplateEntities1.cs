using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using Z.EntityFramework.Plus;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using System.Data;
using Data.Helpers;
using System.Linq;

namespace Data.Model
{
    public partial class TemplateEntities1 : DbContext
    {

        public bool ExecuteStoredProcedure(string ProcedureName, List<Tuple<string, object>> Parameters)
        {
            bool result;
            try
            {
                string command = "exec @Return = " + ProcedureName + " ";
                foreach (Tuple<string, object> var in Parameters)
                {

                    if (var != null && var.Item1 != null && var.Item2 != null)
                        if (var.Item2.GetType() == typeof(string))
                        {
                            command += " " + var.Item1 + "='" + var.Item2.ToString().Replace("'","''") + "',";
                        }
                        else
                        {
                            command += " " + var.Item1 + "=" + var.Item2.ToString() + ",";
                        }

                }
                command = command.Remove(command.Length - 1).Trim();
                SqlParameter parm = new SqlParameter()
                {
                    ParameterName = "@Return",
                    SqlDbType = SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };
                this.Database.ExecuteSqlCommand(command, parm);
                var ResultObject = parm.Value;

                if (ResultObject != null && Convert.ToInt32(ResultObject) == 1)
                    result = true;
                else
                    result = false;
            }
            catch (System.Exception e)
            {
                result = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ProcedureName : " + ProcedureName + " and Parameters.Count = " + Parameters.Count + " and Parameters = " + string.Join(",", Parameters.Select(t => string.Format("[ '{0}', '{1}']", (t.Item1 ?? "NULL"), (t.Item2 ?? "NULL")))));
            }
            return result;
        }

        public override Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

    }
}

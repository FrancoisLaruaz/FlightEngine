using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntities.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data;
using DataEntities.Logger;
using System.Threading;

namespace DataEntities.Model
{
    public partial class TemplateEntities : DbContext
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
                            command += " " + var.Item1 + "='" + var.Item2.ToString() + "',";
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
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ProcedureName : " + ProcedureName + " and Parameters :" + Parameters.ToString());
            }
            return result;
        }



        public override Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }


    }
}

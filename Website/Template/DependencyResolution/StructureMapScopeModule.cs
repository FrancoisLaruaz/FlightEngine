using System.Web;
using Template.App_Start;
using StructureMap.Web.Pipeline;
using System;


namespace Template.DependencyResolution
{

    public class StructureMapScopeModule : IHttpModule
    {
        #region Public Methods and Operators

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            try
            {
                context.BeginRequest += (sender, e) => StructuremapMvc.StructureMapDependencyScope.CreateNestedContainer();
                context.EndRequest += (sender, e) =>
                {
                    HttpContextLifecycle.DisposeAndClearAll();
                    StructuremapMvc.StructureMapDependencyScope.DisposeNestedContainer();
                };
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        #endregion
    }
}
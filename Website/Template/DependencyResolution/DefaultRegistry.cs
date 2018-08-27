using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using DataEntities.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models;
using System.Data.Entity;
using Microsoft.Owin.Security;
using System.Web;
using Identity.Models;
using System;

namespace Template.DependencyResolution
{


    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            try
            {
                Scan(
                    scan =>
                    {
                        scan.Assembly("DataEntities");
                        scan.Assembly("Service");
                        scan.TheCallingAssembly();
                        scan.WithDefaultConventions();
                        scan.With(new ControllerConvention());
                    });
                //For<IExample>().Use<Example>();
                For(typeof(IGenericRepository<>)).Use(typeof(GenericRepository<>));

                For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
                For<DbContext>().Use(() => new ApplicationDbContext());
                For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.BaseType,null);
            }
        }

        #endregion
    }
}
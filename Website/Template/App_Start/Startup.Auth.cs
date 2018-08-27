using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;
using System;
using Identity;
using Identity.Models;
using System.Web.Script.Serialization;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;
using static Website.Controllers.AccountController;
using Microsoft.Owin.Security.Google;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Template
{

    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            try
            {
                // Configure the db context, user manager and signin manager to use a single instance per request
                app.CreatePerOwinContext(ApplicationDbContext.Create);
                app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
                app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

                // Enable the application to use a cookie to store information for the signed in user
                // and to use a cookie to temporarily store information about a user logging in with a third party login provider
                // Configure the sign in cookie
                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Account/Login"),
                    Provider = new CookieAuthenticationProvider
                    {
                        // Enables the application to validate the security stamp when the user logs in.
                        // This is a security feature which is used when you change a password or add an external login to your account.  
                        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                            validateInterval: TimeSpan.FromMinutes(30),
                            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                    }
                });
                app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);


                #region Facebook Config
                string FacebookAppId = ConfigurationManager.AppSettings["FacebookAppId"];
                string FacebookAppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"];
                if (!String.IsNullOrWhiteSpace(FacebookAppId) && !String.IsNullOrWhiteSpace(FacebookAppSecret))
                {
                    var facebookOptions = new FacebookAuthenticationOptions()
                    {
                        AppId = FacebookAppId,
                        AppSecret = FacebookAppSecret,
                    };

                    facebookOptions.BackchannelHttpHandler = new FacebookBackChannelHandler();
                    facebookOptions.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;

                    // Set requested scope
                    facebookOptions.Scope.Add("email");
                    facebookOptions.Scope.Add("user_friends");
                    //facebookOptions.Scope.Add("user_location");
                    // facebookOptions.Scope.Add("user_birthday");

                    app.UseFacebookAuthentication(facebookOptions);

                    #endregion

                #region Google Config
                    string GoogleAppId = ConfigurationManager.AppSettings["GoogleAppId"];
                    string GoogleAppSecret = ConfigurationManager.AppSettings["GoogleAppSecret"];

                    if (!String.IsNullOrWhiteSpace(GoogleAppId) && !String.IsNullOrWhiteSpace(GoogleAppSecret))
                    {


                        var googleOptions = new GoogleOAuth2AuthenticationOptions()
                        {
                            ClientId = GoogleAppId,
                            ClientSecret = GoogleAppSecret,
                            Provider = new GoogleOAuth2AuthenticationProvider()
                            {
                                OnAuthenticated = (context) =>
                                {
                                    context.Identity.AddClaim(new Claim("urn:google:name", context.Identity.FindFirstValue(ClaimTypes.Name)));
                                    context.Identity.AddClaim(new Claim("urn:google:givengame", context.Identity.FindFirstValue(ClaimTypes.GivenName)));
                                    context.Identity.AddClaim(new Claim("urn:google:surname", context.Identity.FindFirstValue(ClaimTypes.Surname)));
                                    context.Identity.AddClaim(new Claim("urn:google:email", context.Identity.FindFirstValue(ClaimTypes.Email)));
                                   // context.Identity.AddClaim(new Claim("urn:google:birthday", context.Identity.FindFirstValue(ClaimTypes.DateOfBirth)));
                                    //This following line is need to retrieve the profile image
                                    context.Identity.AddClaim(new System.Security.Claims.Claim("urn:google:accesstoken", context.AccessToken, ClaimValueTypes.String, "Google"));

                                    return Task.FromResult(0);
                                }
                            }
                        };
                        app.UseGoogleAuthentication(googleOptions);
                        /*
                        app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                        {
                            ClientId = GoogleAppId,
                            ClientSecret = GoogleAppSecret
                        });
                        */
                    }
                    #endregion
                }
            }

            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

        }
    }

}

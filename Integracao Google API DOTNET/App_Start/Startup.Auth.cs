using Integracao_Google_API_DOTNET.Models;
using Integracao_Google_API_DOTNET.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Security.Claims;

namespace Integracao_Google_API_DOTNET
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
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

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            var googleCreds = new GoogleOAuth2AuthenticationOptions
            {

                ClientId = GoogleApi.clientId,
                ClientSecret = GoogleApi.clientSecret,

                Provider = new Microsoft.Owin.Security.Google.GoogleOAuth2AuthenticationProvider
                {
                    OnApplyRedirect = context =>
                    {


                        string redirect = context.RedirectUri;
                        redirect += "&access_type=offline";
                        redirect += "&approval_prompt=force";
                        redirect += "&include_granted_scopes=true";


                        context.Response.Redirect(redirect);

                    },
                    OnAuthenticated = context =>
                    {
                        TimeSpan expiryDuration = context.ExpiresIn ?? new TimeSpan();
                        context.Identity.AddClaim(new Claim("urn:tokens:google:email", context.Email));
                        context.Identity.AddClaim(new Claim("urn:tokens:google:url", context.GivenName));
                        if (!String.IsNullOrEmpty(context.RefreshToken))
                        {
                            context.Identity.AddClaim(new Claim("urn:tokens:google:refreshtoken", context.RefreshToken));
                        }
                        context.Identity.AddClaim(new Claim("urn:tokens:google:accesstoken", context.AccessToken));
                        if (context.User.GetValue("hd") != null)
                        {

                            context.Identity.AddClaim(new Claim("urn:tokens:google:hd", context.User.GetValue("hd").ToString()));
                        }
                        context.Identity.AddClaim(new Claim("urn:tokens:google:accesstokenexpiry", DateTime.UtcNow.Add(expiryDuration).ToString()));

                        return System.Threading.Tasks.Task.FromResult<object>(null);
                    }
                }
            };

            foreach(var scope in GoogleApi.scopes)
            {
                googleCreds.Scope.Add(scope);
            }

            app.UseGoogleAuthentication(googleCreds);
        }
    }
}
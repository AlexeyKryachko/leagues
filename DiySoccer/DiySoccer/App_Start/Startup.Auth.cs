using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Authentication;
using Duke.Owin.VkontakteMiddleware;
using Duke.Owin.VkontakteMiddleware.Provider;
using Interfaces.Users.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace DiySoccer
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and role manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationIdentityContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

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
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<AuthenticationUserManager, UserAuthDb>(
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

            //app.UseGoogleAuthentication();

            var options = new VkAuthenticationOptions
            {
                AppId = "5370269",
                AppSecret = "RMw7P2WLOypLGgzc7fzi",
                Scope = "262144",
                Provider = new VkAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        // Only some of the basic details from facebook 
                        // like id, username, email etc are added as claims.
                        // But you can retrieve any other details from this
                        // raw Json object from facebook and add it as claims here.
                        // Subsequently adding a claim here will also send this claim
                        // as part of the cookie set on the browser so you can retrieve
                        // on every successive request. 
                        var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var userId = HttpContext.Current.User.Identity.GetUserId();
                        var user = userManager.FindByName(context.UserName);
                        if (user == null)
                        {
                            user = new ApplicationUser {
                                UserName = context.UserName,
                                Email = string.IsNullOrEmpty(context.Email) 
                                    ? Guid.NewGuid() + "@diysoccer.ru"
                                    : context.Email 
                            };
                            var result = userManager.Create(user, "0O9i8u#");
                            if (result.Succeeded)
                            {
                                var code = userManager.GenerateEmailConfirmationToken(user.Id);
                                var confirmation = userManager.ConfirmEmail(user.Id, code);
                                if (confirmation.Succeeded)
                                {
                                    context.Identity.AddClaim(new Claim("VkAccessToken", context.AccessToken));
                                    context.Identity.AddClaim(new Claim("VkUserId", context.Id));
                                }
                            }
                        }
                        else
                        {
                            if (!user.EmailConfirmed)
                            {
                                var code = userManager.GenerateEmailConfirmationToken(user.Id);
                                var confirmation = userManager.ConfirmEmail(userId, code);
                                if (confirmation.Succeeded)
                                {
                                    context.Identity.AddClaim(new Claim("VkAccessToken", context.AccessToken));
                                    context.Identity.AddClaim(new Claim("VkUserId", context.Id));
                                }
                            }
                            context.Identity.AddClaim(new Claim("VkAccessToken", context.AccessToken));
                            context.Identity.AddClaim(new Claim("VkUserId", context.Id));
                        }

                        return Task.FromResult(0);
                    }
                }
            };

            app.UseVkontakteAuthentication(options);
        }
    }
}
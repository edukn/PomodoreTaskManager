using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace TaskManagerV2
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //Utilize o seu próprio key de desenvolvedor do Google, Facebook e Twitter.
            app.UseTwitterAuthentication(
               consumerKey: "cChZNFj6T5R0TigYB9yd1w",
               consumerSecret: "L8qq9PZyRg6ieKGEKhZolGC0vJWLw8iEJ88DRdyOg");

            app.UseFacebookAuthentication(
                 appId: "666277200100774",
                 appSecret: "17943b274f9c8dd1acdbdb5d218bfcad");

            app.UseGoogleAuthentication();
            
        }
    }
}
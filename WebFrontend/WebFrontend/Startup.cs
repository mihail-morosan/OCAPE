using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebFrontend.Startup))]
namespace WebFrontend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

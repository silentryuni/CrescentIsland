using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CrescentIsland.Website.Startup))]
namespace CrescentIsland.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

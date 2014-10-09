using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StoreOwnerApp.Startup))]
namespace StoreOwnerApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

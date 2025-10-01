using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TuvVision.Startup))]
namespace TuvVision
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

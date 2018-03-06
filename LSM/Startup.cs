using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LSM.Startup))]
namespace LSM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

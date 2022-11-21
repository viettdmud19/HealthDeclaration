using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DACSN.Startup))]
namespace DACSN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

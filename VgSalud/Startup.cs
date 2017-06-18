using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VgSalud.Startup))]
namespace VgSalud
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

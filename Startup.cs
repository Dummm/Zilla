using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Proect.Startup))]
namespace Proect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Volunteers.Startup))]
namespace Volunteers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

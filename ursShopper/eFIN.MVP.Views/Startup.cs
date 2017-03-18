using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eFIN.MVP.Views.Startup))]
namespace eFIN.MVP.Views
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

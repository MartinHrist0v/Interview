using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Interview.Web.Startup))]
namespace Interview.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

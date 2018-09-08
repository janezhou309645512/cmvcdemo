using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cmvcdemo.Startup))]
namespace cmvcdemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

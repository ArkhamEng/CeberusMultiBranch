using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CerberusMultiBranch.Startup))]
namespace CerberusMultiBranch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

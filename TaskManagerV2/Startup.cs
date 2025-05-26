using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaskManagerV2.Startup))]
namespace TaskManagerV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

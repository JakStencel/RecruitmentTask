using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RecruitmentTask.Startup))]
namespace RecruitmentTask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

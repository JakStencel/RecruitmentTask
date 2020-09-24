using RecruitmentTask.Helpers;
using RecruitmentTask.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace RecruitmentTask
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IAuthService, AuthService>();
            container.RegisterType<IWebConfigReader, WebConfigReader>();
            container.RegisterType<ISSOServcie, SSOServcie>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using Dev.Data.UOW;
using Dev.Services.Contracts;
using Dev.Services.Managers;

namespace DevApplication
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IUnitOfWork, UnitofWork>();
            container.RegisterType<IDevTestServices, DevTestServices>();     

            return container;
        }
    }
}
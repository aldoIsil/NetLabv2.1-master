using BusinessLayer;
using NetLab.Repository.Infrastructure;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace NetLab
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IConnectionFactory, ConnectionFactory>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            //container.RegisterType<IEstablecimientoRepository, EstablecimientoRepository>();
            //container.RegisterType<EstablecimientosBL>(new InjectionConstructor(0));
            container.RegisterType<IEstablecimientosBL, EstablecimientosBL>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}
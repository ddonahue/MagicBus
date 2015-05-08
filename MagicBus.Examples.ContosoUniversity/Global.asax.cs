using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MagicBus;
using MagicBus.HandlerFactory.Ninject;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;

namespace ContosoUniversity
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : NinjectHttpApplication
    {
		protected override void OnApplicationStarted()
        {
			base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

		protected override IKernel CreateKernel()
		{
			var kernel = new StandardKernel();

			kernel.Bind(x => x.FromThisAssembly()
							.SelectAllClasses()
							.InheritedFrom(typeof(IHandler<>))
							.BindDefaultInterfaces());
			kernel.Bind<IBus>().To<Bus>();
			kernel.Bind<IHandlerFactory>().To<NinjectHandlerFactory>();

			return kernel;
		}
    }
}
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MagicBus.Example.Web.DependencyResolution;
using StructureMap;
using StructureMap.Graph;

namespace MagicBus.Example.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		private static void InitializeStructureMap()
		{
			ObjectFactory.Initialize(x =>
			{
				x.For<IBus>().Use<Bus>();
			});
			ObjectFactory.Configure(x => x.Scan(scanner =>
			{
				scanner.AssemblyContainingType<Bus>();
				scanner.TheCallingAssembly();
				scanner.WithDefaultConventions();
				scanner.ConnectImplementationsToTypesClosing(typeof(IHandler<>));
			}));

			ObjectFactory.Configure(x => x.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t)));
			ObjectFactory.Configure(x => x.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t)));

			var container = ObjectFactory.Container;
			GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);
		}
	}
}
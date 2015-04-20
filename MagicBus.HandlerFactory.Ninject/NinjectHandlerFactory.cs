using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Infrastructure.Language;

namespace MagicBus.HandlerFactory.Ninject
{
    public class NinjectHandlerFactory : IHandlerFactory
    {
	    private readonly IKernel kernel;

	    public NinjectHandlerFactory(IKernel kernel)
	    {
		    this.kernel = kernel;
	    }

	    public IHandler<T> GetCommandHandler<T>(T command) where T : ICommand
	    {
		    return kernel.Get<IHandler<T>>();
	    }

	    public IEnumerable<IHandler<T>> GetEventHandlers<T>(T @event) where T : IEvent
	    {
		    return GetHandlersForAllInterfacesOf<T>()
				.Concat(GetEventHandlersForAllBaseClassesOf<T>());
	    }

	    private IEnumerable<IHandler<T>> GetEventHandlersForAllBaseClassesOf<T>() where T : IEvent
	    {
			var baseTypesOfT = typeof(T)
				 .GetAllBaseTypes()
				 .Where(x => x != typeof(object));

			var handlersOfT = baseTypesOfT.Select(type => typeof(IHandler<>).MakeGenericType(type));

			return handlersOfT.SelectMany(type => kernel.GetAll(type).Cast<IHandler<T>>());
	    }

	    private IEnumerable<IHandler<T>> GetHandlersForAllInterfacesOf<T>() where T : IEvent
	    {
			var interfacesOfT = typeof(T).GetInterfaces();

			var handlersOfT = interfacesOfT.Select(type => typeof(IHandler<>).MakeGenericType(type));

			return handlersOfT.SelectMany(type => kernel.GetAll(type).Cast<IHandler<T>>());
	    }
    }
}

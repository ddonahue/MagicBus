using System.Collections.Generic;

namespace MagicBus
{
	public interface IHandlerFactory
	{
		IHandler<T> GetCommandHandler<T>(T command) where T : ICommand;
		IEnumerable<IHandler<T>> GetEventHandlers<T>(T @event) where T : IEvent;
	}
}

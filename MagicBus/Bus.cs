using System;
using System.Collections.Generic;
using System.Linq;
using MagicBus.Exceptions;
using MagicBus.Extensions;

namespace MagicBus
{
	public delegate object SingleInstanceFactory(Type serviceType);
	public delegate IEnumerable<object> MultiInstanceFactory(Type serviceType);

	public class Bus : IBus
	{
		private readonly SingleInstanceFactory commandHandlerFactory;
		private readonly MultiInstanceFactory eventHandlersFactory;

		public Bus(SingleInstanceFactory commandHandlerFactory, MultiInstanceFactory eventHandlersFactory)
		{
			this.commandHandlerFactory = commandHandlerFactory;
			this.eventHandlersFactory = eventHandlersFactory;
		}

		public void Send(params ICommand[] commands)
		{
			if (commands.IsNullOrEmpty())
				throw new ArgumentException("The commands parameter is null or empty.");

			foreach (var command in commands)
			{
				var handler = GetHandlerFor(command);
				handler.Handle(command);
			}
		}

		public void Publish(params IEvent[] events)
		{
			if (events.IsNullOrEmpty())
				throw new ArgumentException("The events parameter is null or empty.");

			foreach (var @event in events)
				foreach (var handler in GetHandlersFor(@event))
					handler.Handle(@event);
		}

		private IHandler<T> GetHandlerFor<T>(T command) where T : class, ICommand
		{
			var handlerType = typeof(IHandler<>).MakeGenericType(command.GetType());
			var handlerWrapperType = typeof (HandlerWrapper<>).MakeGenericType(command.GetType());

			var handler = commandHandlerFactory(handlerType);
			if (handler == null)
				throw new NoHandlerFoundException(command.GetType());

			return (IHandler<T>)Activator.CreateInstance(handlerWrapperType, handler);
		}

		private IEnumerable<IHandler<T>> GetHandlersFor<T>(T @event) where T : class, IEvent
		{
			var handlerType = typeof(IHandler<>).MakeGenericType(@event.GetType());
			var handlerWrapperType = typeof(HandlerWrapper<>).MakeGenericType(@event.GetType());

			var handlers = eventHandlersFactory(handlerType);
			
			return handlers.Select(handler => (IHandler<T>)Activator.CreateInstance(handlerWrapperType, handler));
		}

		private class HandlerWrapper<TMessage> : IHandler<IMessage> where TMessage : IMessage
		{
			private readonly IHandler<TMessage> inner;

			public HandlerWrapper(IHandler<TMessage> inner)
			{
				this.inner = inner;
			}

			public void Handle(IMessage message)
			{
				inner.Handle((TMessage)message);
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Reflection;
using MagicBus.Exceptions;
using MagicBus.Extensions;

namespace MagicBus
{
	public class Bus : IBus
	{
		private readonly IHandlerFactory handlerFactory;

		public Bus(IHandlerFactory handlerFactory)
		{
			this.handlerFactory = handlerFactory;
		}

		public void Send(params ICommand[] commands)
		{
			if (commands.IsNullOrEmpty())
				throw new ArgumentException("The commands parameter is null or empty.");

			var sendInternal = typeof(Bus).GetMethod("SendInternal", BindingFlags.Instance | BindingFlags.NonPublic);

			ExecuteMessageHandlers(commands, sendInternal);
		}

		public void Publish(params IEvent[] events)
		{
			if (events.IsNullOrEmpty())
				throw new ArgumentException("The events parameter is null or empty.");

			var publishInternal = typeof(Bus).GetMethod("PublishInternal", BindingFlags.Instance | BindingFlags.NonPublic);

			ExecuteMessageHandlers(events, publishInternal);
		}

		private void ExecuteMessageHandlers(IEnumerable<IMessage> messages, MethodInfo internalMethod)
		{
			foreach (var message in messages)
			{
				try
				{
					internalMethod
						.MakeGenericMethod(message.GetType())
						.Invoke(this, new object[] {message});
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
			}
		}

		// ReSharper disable once UnusedMember.Local
		private void SendInternal<TCommand>(TCommand command) where TCommand : class, ICommand
		{
			var handler = handlerFactory.GetCommandHandler(command);
			if (handler == null)
				throw new NoHandlerFoundException<TCommand>();

			handler.Handle(command);
		}

		// ReSharper disable once UnusedMember.Local
		private void PublishInternal<TEvent>(TEvent @event) where TEvent : class, IEvent
		{
			var handlers = handlerFactory.GetEventHandlers(@event);
			foreach (var handler in handlers)
				handler.Handle(@event);
		}
	}
}
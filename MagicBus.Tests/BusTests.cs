using System;
using System.Collections.Generic;
using System.Linq;
using MagicBus.Exceptions;
using NUnit.Framework;

namespace MagicBus.Tests
{
	[TestFixture]
    public class BusTests
	{
		[Test]
		public void Send_NullCommandsParameter_ShouldThrowException()
		{
			var sut = new Bus(new FakeHandlerFactory());
			Assert.Throws<ArgumentException>(() => sut.Send(null), "The commands parameter is null or empty.");
		}

		[Test]
		public void Send_EmptyCommandsParameter_ShouldThrowException()
		{
			var sut = new Bus(new FakeHandlerFactory());
			Assert.Throws<ArgumentException>(() => sut.Send(new List<ICommand>().ToArray()), "The commands parameter is null or empty.");
		}

		[Test]
		public void Send_NoHandlerFoundForCommand_ShouldThrowException()
		{
			var sut = new Bus(new FakeHandlerFactory());
			Assert.Throws<NoHandlerFoundException<NoMatchingHandlerCommand>>(() => sut.Send(new NoMatchingHandlerCommand()));
		}

		[Test]
		public void Send_HandlerFound_ShouldCallHandle()
		{
			var handler = new MatchingHandler();
			var sut = new Bus(new FakeHandlerFactory(handler, null));

			sut.Send(new MatchingHandlerCommand());

			Assert.AreEqual(1, handler.CallCount);	
		}

		[Test]
		public void Publish_NullEventsParameter_ShouldThrowException()
		{
			var sut = new Bus(new FakeHandlerFactory());
			Assert.Throws<ArgumentException>(() => sut.Publish(null), "The events parameter is null or empty.");
		}

		[Test]
		public void Publish_EmptyEventsParameter_ShouldThrowException()
		{
			var sut = new Bus(new FakeHandlerFactory());
			Assert.Throws<ArgumentException>(() => sut.Publish(new List<IEvent>().ToArray()), "The events parameter is null or empty.");
		}

		[Test]
		public void Publish_ShouldCallHandleOnEachHandler()
		{
			var handler1 = new MatchingEventHandler1();
			var handler2 = new MatchingEventHandler2();
			var sut = new Bus(new FakeHandlerFactory(null, new IHandler<MatchingHandlerEvent>[] { handler1, handler2 }));

			sut.Publish(new MatchingHandlerEvent());

			Assert.AreEqual(1, handler1.CallCount);	
			Assert.AreEqual(1, handler2.CallCount);	
		}
    }

	public class MatchingEventHandler1 : IHandler<MatchingHandlerEvent>
	{
		public int CallCount { get; private set; }

		public void Handle(MatchingHandlerEvent message)
		{
			CallCount++;
		}
	}

	public class MatchingEventHandler2 : IHandler<MatchingHandlerEvent>
	{
		public int CallCount { get; private set; }

		public void Handle(MatchingHandlerEvent message)
		{
			CallCount++;
		}
	}

	public class MatchingHandlerEvent : IEvent { }



	public class NoMatchingHandlerEvent : IEvent { }

	public class NoMatchingHandlerCommand : ICommand { }



	public class MatchingHandlerCommand : ICommand {}

	public class MatchingHandler : IHandler<MatchingHandlerCommand>
	{
		public int CallCount { get; private set; }

		public void Handle(MatchingHandlerCommand message)
		{
			CallCount++;
		}
	}



	public class FakeHandlerFactory : IHandlerFactory
	{
		private readonly IHandler<MatchingHandlerCommand> commandHandler;
		private readonly IEnumerable<IHandler<MatchingHandlerEvent>> eventHandlers;

		public FakeHandlerFactory() : this(null, null) { }

		public FakeHandlerFactory(IHandler<MatchingHandlerCommand> commandHandler, IEnumerable<IHandler<MatchingHandlerEvent>> eventHandlers)
		{
			this.commandHandler = commandHandler;
			this.eventHandlers = eventHandlers;
		}

		public IHandler<T> GetCommandHandler<T>(T command) where T : ICommand
		{
			if (command is MatchingHandlerCommand)
				return (IHandler<T>) commandHandler;
			
			return null;
		}

		public IEnumerable<IHandler<T>> GetEventHandlers<T>(T @event) where T : IEvent
		{
			if (@event is MatchingHandlerEvent)
				return eventHandlers.Cast<IHandler<T>>();

			return new List<IHandler<T>>();
		}
	}
}

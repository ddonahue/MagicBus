using System;
using System.Collections.Generic;
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
			var sut = new Bus(new HandlerFactoryTestHelper().GetCommandHandler, null);
			Assert.Throws<ArgumentException>(() => sut.Send(null), "The commands parameter is null or empty.");
		}

		[Test]
		public void Send_EmptyCommandsParameter_ShouldThrowException()
		{
			var sut = new Bus(new HandlerFactoryTestHelper().GetCommandHandler, null);
			Assert.Throws<ArgumentException>(() => sut.Send(new List<ICommand>().ToArray()), "The commands parameter is null or empty.");
		}

		[Test]
		public void Send_NoHandlerFoundForCommand_ShouldThrowException()
		{
			var sut = new Bus(new HandlerFactoryTestHelper().GetCommandHandler, null);
			Assert.Throws<NoHandlerFoundException>(() => sut.Send(new NoMatchingHandlerCommand()));
		}

		[Test]
		public void Send_HandlerFound_ShouldCallHandle()
		{
			var handler = new MatchingHandler();
			var sut = new Bus(new HandlerFactoryTestHelper(handler).GetCommandHandler, null);

			sut.Send(new MatchingHandlerCommand());

			Assert.AreEqual(1, handler.CallCount);	
		}

		[Test]
		public void Publish_NullEventsParameter_ShouldThrowException()
		{
			var sut = new Bus(null, new HandlerFactoryTestHelper().GetEventHandlers);
			Assert.Throws<ArgumentException>(() => sut.Publish(null), "The events parameter is null or empty.");
		}

		[Test]
		public void Publish_EmptyEventsParameter_ShouldThrowException()
		{
			var sut = new Bus(null, new HandlerFactoryTestHelper().GetEventHandlers);
			Assert.Throws<ArgumentException>(() => sut.Publish(new List<IEvent>().ToArray()), "The events parameter is null or empty.");
		}

		[Test]
		public void Publish_ShouldCallHandleOnEachHandler()
		{
			var handler1 = new MatchingEventHandler1();
			var handler2 = new MatchingEventHandler2();
			var sut = new Bus(null, new HandlerFactoryTestHelper(new IHandler<MatchingHandlerEvent>[] { handler1, handler2 }).GetEventHandlers);

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

	public class HandlerFactoryTestHelper
	{
		private readonly IHandler<MatchingHandlerCommand> commandHandler;
		private readonly IEnumerable<IHandler<MatchingHandlerEvent>> eventHandlers;

		public HandlerFactoryTestHelper() : this(null, null) { }

		public HandlerFactoryTestHelper(IHandler<MatchingHandlerCommand> commandHandler) : this(commandHandler, null)
		{
		}

		public HandlerFactoryTestHelper(IEnumerable<IHandler<MatchingHandlerEvent>> eventHandlers) : this(null, eventHandlers)
		{
		}

		private HandlerFactoryTestHelper(IHandler<MatchingHandlerCommand> commandHandler, IEnumerable<IHandler<MatchingHandlerEvent>> eventHandlers)
		{
			this.commandHandler = commandHandler;
			this.eventHandlers = eventHandlers;
		}

		public object GetCommandHandler(Type t)
		{
			return (t == typeof(IHandler<MatchingHandlerCommand>)) ? commandHandler : null;
		}

		public IEnumerable<object> GetEventHandlers(Type t)
		{
			return (t == typeof(IHandler<MatchingHandlerEvent>)) ? eventHandlers : null;
		}
	}
}

using System;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Planning.Bindings.Resolvers;

namespace MagicBus.Example.Ninject
{
	class Program
	{
		static void Main()
		{
			var bus = BootstrapBus();

			var command = new TestCommand {Number = 42};
			bus.Send(command);

			var @event = new TestEvent {Letter = 'D' };
			bus.Publish(@event);

			Console.ReadLine();
		}

		private static IBus BootstrapBus()
		{
			var kernel = new StandardKernel();
			kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();
			kernel.Bind(scan => scan.FromAssemblyContaining<IBus>().SelectAllClasses().BindDefaultInterface());
			kernel.Bind(scan => scan.FromThisAssembly().SelectAllClasses().BindAllInterfaces());
			kernel.Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
			kernel.Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));

			return kernel.Get<IBus>();
		}
	}

	public class TestCommand : ICommand
	{
		public int Number { get; set; }
	}

	public class TestCommandHandler : IHandler<TestCommand>
	{
		public void Handle(TestCommand command)
		{
			Console.WriteLine("The command I received was: {0}", command.Number);
		}
	}

	public class TestEvent : IEvent
	{
		public char Letter { get; set; }
	}

	public class TestEventHandler : IHandler<TestEvent>
	{
		public void Handle(TestEvent @event)
		{
			Console.WriteLine("The event I received was: {0}", @event.Letter);
		}
	}

	public class TestIEventHandler : IHandler<IEvent>
	{
		public void Handle(IEvent message)
		{
			Console.WriteLine("Handle was called in IHandler<IEvent> catchall.");
		}
	}
}

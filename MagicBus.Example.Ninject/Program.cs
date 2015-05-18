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

			var message = new TestMessage {Number = 42};
			bus.Send(message);
		}

		private static IBus BootstrapBus()
		{
			var kernel = new StandardKernel();
			kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();
			kernel.Bind(scan => scan.FromAssemblyContaining<IBus>().SelectAllClasses().BindDefaultInterface());
			kernel.Bind(scan => scan.FromAssemblyContaining<TestMessage>().SelectAllClasses().BindAllInterfaces());
			kernel.Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
			kernel.Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));

			return kernel.Get<IBus>();
		}
	}

	public class TestMessage : ICommand
	{
		public int Number { get; set; }
	}

	public class TestMessageHandler : IHandler<TestMessage>
	{
		public void Handle(TestMessage message)
		{
			Console.WriteLine("The message I received was: {0}", message.Number);
		}
	}
}

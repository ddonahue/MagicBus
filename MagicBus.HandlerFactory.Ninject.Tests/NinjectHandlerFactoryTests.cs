using System.Linq;
using Ninject;
using Ninject.Extensions.Conventions;
using NUnit.Framework;

namespace MagicBus.HandlerFactory.Ninject.Tests
{
	[TestFixture]
    public class NinjectHandlerFactoryTests
	{
		private StandardKernel kernel;
		private NinjectHandlerFactory sut;

		[SetUp]
		public void SetUp()
		{
			kernel = new StandardKernel();
			kernel.Bind(x => x.FromThisAssembly()
							.SelectAllClasses()
							.InheritedFrom(typeof(IHandler<>))
							.BindDefaultInterfaces());

			sut = new NinjectHandlerFactory(kernel);
		}

		[Test]
		public void GetCommandHandler_ReturnsCorrectHandler()
		{
			var result = sut.GetCommandHandler(new FakeCommand());

			Assert.IsInstanceOf<FakeCommandHandler>(result);
		}

		[Test]
		public void GetEventHandlers_ReturnsCorrectHandlers_ForConcreteType()
		{
			var result = sut.GetEventHandlers(new FakeEvent());

			Assert.AreEqual(3, result.Count());
			Assert.IsTrue(result.Any(x => x is FakeEventHandler1));
			Assert.IsTrue(result.Any(x => x is FakeEventHandler2));
		}

		[Test]
		public void GetEventHandlers_ReturnsCorrectHandlers_ForIEvent()
		{
			var result = sut.GetEventHandlers(new FakeEvent());

			Assert.AreEqual(3, result.Count());
			Assert.IsTrue(result.Any(x => x is FakeEventHandler3));
		}

		[Test]
		public void GetEventHandlers_ReturnCorrectHandlers_ForBaseEventClass()
		{
			var message = new SubClassOfFakeEvent();
			var result = sut.GetEventHandlers(message);

			Assert.AreEqual(4, result.Count());
			Assert.IsTrue(result.Any(x => x is FakeEventHandler1));
		}
    }

	public class FakeCommandHandler : IHandler<FakeCommand>

	{
		public void Handle(FakeCommand message)
		{
			throw new System.NotImplementedException();
		}
	}

	public class FakeCommand : ICommand { }



	public class FakeEvent : IEvent { }

	public class SubClassOfFakeEvent : FakeEvent { }

	public class FakeEventHandler1 : IHandler<FakeEvent>
	{
		public void Handle(FakeEvent message)
		{
			return;
		}
	}

	public class FakeEventHandler2 : IHandler<FakeEvent>
	{
		public void Handle(FakeEvent message)
		{
			return;
		}
	}

	public class FakeEventHandler3 : IHandler<IEvent>
			
	{
		public void Handle(IEvent message)
		{
			return;
		}
	}

	public class FakeEventHandler4 : IHandler<SubClassOfFakeEvent>
	{
		public void Handle(SubClassOfFakeEvent message)
		{
			return;
		}
	}
}


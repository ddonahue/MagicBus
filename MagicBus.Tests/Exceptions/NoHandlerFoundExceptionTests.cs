using MagicBus.Exceptions;
using NUnit.Framework;

namespace MagicBus.Tests.Exceptions
{
	[TestFixture]
	public class NoHandlerFoundExceptionTests
	{
		[Test]
		public void ShouldUseMessageTypeNameInErrorMessage()
		{
			var sut = new NoHandlerFoundException(typeof(NoMatchingHandlerCommand));
			Assert.AreEqual("No handlers registered for NoMatchingHandlerCommand", sut.Message);
		}
	}

	public class NoMatchingHandlerCommand : ICommand
	{
	}
}
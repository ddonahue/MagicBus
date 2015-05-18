using System;

namespace MagicBus.Exceptions
{
	public class NoHandlerFoundException : Exception
	{
		public NoHandlerFoundException(Type messageType) : base(string.Format("No handlers registered for {0}", messageType.Name))
		{
		}
	}
}
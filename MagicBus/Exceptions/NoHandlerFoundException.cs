using System;

namespace MagicBus.Exceptions
{
	public class NoHandlerFoundException<TMessage> : Exception where TMessage : IMessage
	{
		public NoHandlerFoundException() : base(string.Format("No handlers registered for {0}", typeof(TMessage).Name))
		{
		}
	}
}
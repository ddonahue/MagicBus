using System;

namespace MagicBus.Exceptions
{
	public class NoHandlerFoundException : Exception
	{
		public NoHandlerFoundException(Type t) : base(string.Format("No handlers registered for {0}", t.Name))
		{
		}
	}
}
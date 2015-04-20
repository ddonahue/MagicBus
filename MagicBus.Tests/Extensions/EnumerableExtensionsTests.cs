using System.Collections.Generic;
using MagicBus.Extensions;
using NUnit.Framework;

namespace MagicBus.Tests.Extensions
{
	[TestFixture]
	public class EnumerableExtensionsTests
	{
		[Test]
		public void IsNull_ReturnsTrue()
		{
			var result = EnumerableExtensions.IsNullOrEmpty<int>(null);
			Assert.IsTrue(result);
		}

		[Test]
		public void IsEmpty_ReturnsTrue()
		{
			var result = EnumerableExtensions.IsNullOrEmpty(new List<int>());
			Assert.IsTrue(result);
		}

		[Test]
		public void HasItems_ReturnsFalse()
		{
			var result = EnumerableExtensions.IsNullOrEmpty(new List<int> { 1,3 });
			Assert.IsFalse(result);
		}
	}
}

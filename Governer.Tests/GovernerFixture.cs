using System;
using NUnit.Framework;

namespace Governer.Tests
{
	[TestFixture]
	public class GovernerFixture
	{
		[Test]
		public void GovernerTest ()
		{
			var governer = new Governer ("tenant-x-multi-avail", 25, new TimeSpan (0, 5, 0));
			Assert.AreEqual (true, governer.IsAllowed ());
		}

		[Test]
		public void ExceedingRateShouldNotBeAllowedTest ()
		{
			var governer = new Governer ("tenant-x-multi-avail", 0, new TimeSpan (0, 5, 0));
			Assert.AreEqual (false, governer.IsAllowed ());
		}
	}
}


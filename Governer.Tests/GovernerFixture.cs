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
			var gauge = new Gauge ("tenant-x-multi-avail", 5 * 60);
			var governer = new Governer (gauge, 25);
			Assert.AreEqual (true, governer.IsAllowed ());
		}

		[Test]
		public void ExceedingRateShouldNotBeAllowedTest ()
		{
			var gauge = new Gauge ("tenant-x-multi-avail", 5 * 60);
			var governer = new Governer (gauge, 0);
			Assert.AreEqual (false, governer.IsAllowed ());
		}
	}
}


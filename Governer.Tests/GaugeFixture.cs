using System;
using NUnit.Framework;
using System.Threading;

namespace Governer.Tests
{
	[TestFixture]
	public class GaugeFixture
	{
		[Test]
		public void IncrementWillInitializeGaugeTest()
		{
			var gauge = new Gauge("tenant-x-multi-avail", 15);
			Assert.AreEqual(1, gauge.Increment());
		}

		[Test]
		public void SequentialIncrementShouldIncrementCountTest()
		{
			var gauge = new Gauge("tenant-x-multi-avail", 15);
			Assert.AreEqual(1, gauge.Increment());
			Assert.AreEqual(2, gauge.Increment());
		}

		[Test]
		public void CountShouldResetAfterWindowInterval()
		{
			var gauge = new Gauge("tenant-x-multi-avail", 2);
			Assert.AreEqual(1, gauge.Increment());
			Thread.Sleep (2500);
			Assert.AreEqual(1, gauge.Increment());
		}
	}
}


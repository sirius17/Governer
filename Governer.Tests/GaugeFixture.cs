using System;
using NUnit.Framework;
using System.Threading;
using Moq;

namespace Governer.Tests
{
	[TestFixture]
	public class GaugeFixture
	{
		[Test]
		public void IncrementWillInitializeGaugeTest()
		{
			var gaugeName = Guid.NewGuid ().ToString ("N");
			var gauge = new Gauge(gaugeName, 15);
			Assert.AreEqual(1, gauge.Increment());
		}

		[Test]
		public void SequentialIncrementShouldIncrementCountTest()
		{
			var gaugeName = Guid.NewGuid ().ToString ("N");
			var gauge = new Gauge(gaugeName, 15);
			Assert.AreEqual(1, gauge.Increment());
			Assert.AreEqual(2, gauge.Increment());
		}

		[Test]
		public void CountShouldResetAfterWindowInterval()
		{
			var gaugeName = Guid.NewGuid ().ToString ("N");
			var gauge = new Gauge(gaugeName, 2);
			Assert.AreEqual(1, gauge.Increment());
			Thread.Sleep (2500);
			Assert.AreEqual(1, gauge.Increment());
		}
	}
}


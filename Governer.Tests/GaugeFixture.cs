using System;
using NUnit.Framework;
using System.Threading;
using Moq;
using Governer.Internal;

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

		[Test]
		public void GaugeNamesAreCaseInsensitiveTest()
		{
			var gaugeName = Guid.NewGuid ().ToString ("N") + "TESTKEY";
			var gauge1 = new Gauge(gaugeName, 15);
			var gauge2 = new Gauge(gaugeName.ToLower(), 15);
			Assert.AreEqual(1, gauge1.Increment());
			Assert.AreEqual(2, gauge2.Increment());
		}

		[Test]
		public void GaugeWillReturn0OnFaultTest()
		{
			var storageMock = new Mock<IGaugeStorage> ();
			storageMock.Setup (m => m.Increment (It.IsAny<string> (), It.IsAny<ulong> ())).Throws (new Exception ("Something failed."));
			var gaugeName = Guid.NewGuid ().ToString ("N");
			var gauge1 = new Gauge(gaugeName, 15, null, storageMock.Object);
			Assert.AreEqual(0, gauge1.Increment());
		}
	}
}


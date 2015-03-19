using System;
using NUnit.Framework;
using Moq;

namespace Governer.Tests
{
	[TestFixture]
	public class IntegrationFixture
	{
		[Test]
		public void SingleMachineOperationTest()
		{
			var gaugeA = new Gauge ("search-api-tenant-x", 4);
			var gaugeB = new Gauge ("search-api-tenant-x", 4);
			var governerA = new Governer (gaugeA, 5);
			var governerB = new Governer (gaugeB, 5);
			//Essentially we are saying that we allow  20 requests in a 4 second window.
			for (int i = 0; i < 10; i++) 
			{
				Assert.IsTrue( governerA.IsAllowed ());
				Assert.IsTrue( governerB.IsAllowed ());
			}
			Assert.IsFalse( governerA.IsAllowed ());
			Assert.IsFalse( governerB.IsAllowed ());
		}

		[Test]
		public void MultiMachineOperationTest()
		{
			var clockA = new Clock ();
			var clockB = new Clock ();
			var gaugeA = new Gauge ("search-api-tenant-y", 4, clockA);
			var gaugeB = new Gauge ("search-api-tenant-y", 4, clockB);
			var governerA = new Governer (gaugeA, 5);
			var governerB = new Governer (gaugeB, 5);
			//Essentially we are saying that we allow  20 requests in a 4 second window.
			for (int i = 0; i < 10; i++) 
			{
				Assert.IsTrue( governerA.IsAllowed ());
				Assert.IsTrue( governerB.IsAllowed ());
			}
			Assert.IsFalse( governerA.IsAllowed ());
			Assert.IsFalse( governerB.IsAllowed ());
		}

		[Test]
		public void MultiMachineWithDifferentClocksOperationTest()
		{
			// Scenario
			// Single time server which is on current time.
			// Two machines 
			// 1. Machine A which is 10 sec behind the time server.
			// 2. Machine B is 10 sec ahead of time server.

			// Time server time is essentiall the current time.
			// Time server should be 10 seconds ahead of machine A.
			var timeServerAMock = new Mock<ITimeServer>();
			timeServerAMock
				.Setup (t => t.GetCurrentUtcTime (It.IsAny<DateTime> ()))
				.Returns<DateTime> (t => t.AddSeconds(10));
			var timeServiceA = new TimeService (new [] { timeServerAMock.Object });
			// Local clock of machine A is 10 seconds behind the time server time.
			var timeSourceAMock = new Mock<ITimeSource> ();
			timeSourceAMock
				.Setup (t => t.GetUtcNow ())
				.Returns (() => DateTime.UtcNow.AddSeconds (-10));

			// Time server should be 10 seconds behind of machine B.
			var timeServerBMock = new Mock<ITimeServer>();
			timeServerBMock
				.Setup (t => t.GetCurrentUtcTime (It.IsAny<DateTime> ()))
				.Returns<DateTime> (t => t.AddSeconds(-10));
			var timeServiceB = new TimeService (new [] { timeServerBMock.Object });
			// Local clock of machine B is 10 seconds ahead the time server time.
			var timeSourceBMock = new Mock<ITimeSource> ();
			timeSourceBMock
				.Setup (t => t.GetUtcNow ())
				.Returns (() => DateTime.UtcNow.AddSeconds (10));

			var clockA = new Clock (timeServiceA, timeSource: timeSourceAMock.Object);
			var clockB = new Clock (timeServiceB, timeSource: timeSourceBMock.Object);
			var gaugeA = new Gauge ("search-api-tenant-z", 4, clockA);
			var gaugeB = new Gauge ("search-api-tenant-z", 4, clockB);
			var governerA = new Governer (gaugeA, 5);
			var governerB = new Governer (gaugeB, 5);
			//Essentially we are saying that we allow  20 requests in a 4 second window.
			for (int i = 0; i < 10; i++) 
			{
				Assert.IsTrue( governerA.IsAllowed ());
				Assert.IsTrue( governerB.IsAllowed ());
			}
			Assert.IsFalse( governerA.IsAllowed ());
			Assert.IsFalse( governerB.IsAllowed ());
		}
	}
}


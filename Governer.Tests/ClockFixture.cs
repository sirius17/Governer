using System;
using NUnit.Framework;
using Moq;
using Governer.Internal;

namespace Governer.Tests
{
	[TestFixture]
	public class ClockFixture
	{
		[Test]
		public void GetUtcNowTest()
		{
			var clock = new Clock ();
			var currentTime = DateTime.UtcNow;
			var clockTime = clock.UtcNow;
			var timeDifference = clockTime - currentTime;
			Assert.IsTrue (timeDifference.Duration().TotalSeconds <= 1.0d);
		}

		[Test]
		public void ClockWithTimeServiceTest()
		{
			var mockServer = new Mock<ITimeServer> ();
			mockServer
				.Setup (m => m.GetCurrentUtcTime (It.IsAny<DateTime> ()))
				.Returns<DateTime> (t => t.AddSeconds(5));
			var timeService = new TimeService( new [] { mockServer.Object });
			var clock = new Clock (timeService);
			var difference = Convert.ToInt32 ((clock.UtcNow - DateTime.UtcNow).TotalSeconds);
			Assert.AreEqual (5, difference);
		}

		[Test]
		public void ClockShouldUseTimeSourceTest()
		{
			var utcNow = DateTime.UtcNow.AddDays (-10);
			var timeSourceMock = new Mock<ITimeSource> ();
			timeSourceMock
				.Setup (x => x.GetUtcNow ())
				.Returns (utcNow);
			var clock = new Clock (timeSource: timeSourceMock.Object);
			Assert.AreEqual (utcNow, clock.GetSystemUtcTime ());
		}


	}
}


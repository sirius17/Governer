using NUnit.Framework;
using System;
using Moq;
using System.Threading;
using Governer.Internal;

namespace Governer.Tests
{
	[TestFixture ()]
	public class TimeServiceFixture
	{
		[Test ()]
		public void InitializeTimeServiceTest ()
		{
			var utcNow = DateTime.UtcNow;
			var serverMock = new Mock<ITimeServer> ();
			serverMock
				.Setup (s => s.GetCurrentUtcTime ())
				.Returns(() => DateTime.UtcNow);
			ITimeServer[] servers = new [] { serverMock.Object };
			var service = new TimeService (servers);
			var correction = service.GetClockOffset (DateTime.UtcNow);
			Assert.AreEqual (0, Convert.ToInt32(correction.TotalSeconds));
		}


		[Test ()]
		public void OffsetShouldBePositiveWhenServerIsAheadTest ()
		{
			var now = DateTime.UtcNow;
			var timerMock = new Mock<global::Governer.Internal.Timer> ();
			timerMock
				.Setup (x => x.MeasureInSeconds (It.IsAny<Action> ()))
				.Returns<Action> (x => {
						x();	
						return 2;
					});
			
			var serverMock = new Mock<ITimeServer> ();
			serverMock
				.Setup (s => s.GetCurrentUtcTime ())
				.Returns(() =>  now.AddSeconds(6));
			
			ITimeServer[] servers = new [] { serverMock.Object };
			var service = new TimeService (servers, timerMock.Object);
			var correction = service.GetClockOffset (now);
			Assert.AreEqual (5, Convert.ToInt32(correction.TotalSeconds));
		}

		[Test ()]
		public void OffsetShouldBeNegativeWhenServerIsBehindTest ()
		{
			var now = DateTime.UtcNow;
			var timerMock = new Mock<global::Governer.Internal.Timer> ();
			timerMock
				.Setup (x => x.MeasureInSeconds (It.IsAny<Action> ()))
				.Returns<Action> (x => {
					x();	
					return 2;
				});

			var serverMock = new Mock<ITimeServer> ();
			serverMock
				.Setup (s => s.GetCurrentUtcTime ())
				.Returns(() =>  now.AddSeconds(-6));
			ITimeServer[] servers = new [] { serverMock.Object };
			var service = new TimeService (servers, timerMock.Object);
			var correction = service.GetClockOffset (DateTime.UtcNow);
			Assert.AreEqual (-5, Convert.ToInt32(correction.TotalSeconds));
		}

		[Test]
		public void OffsetShouldBeZeroIfNoTimeServersAreProvided()
		{
			var service = new TimeService (null, null);
			Assert.AreEqual (TimeSpan.Zero, service.GetClockOffset (DateTime.UtcNow));

			service = new TimeService (new ITimeServer[] {}, null);
			Assert.AreEqual (TimeSpan.Zero, service.GetClockOffset (DateTime.UtcNow));
		}

		[Test]
		public void OffsetShouldBeZeroIfAllTimeServersAreFaultingTest()
		{
			var serverMock = new Mock<ITimeServer> ();
			serverMock
				.Setup (s => s.GetCurrentUtcTime ())
				.Throws (new Exception ("Server unavailable"));
			var service = new TimeService (new ITimeServer[] { serverMock.Object });
			Assert.AreEqual (TimeSpan.Zero, service.GetClockOffset (DateTime.UtcNow));
		}
	}
}


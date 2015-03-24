using System;
using NUnit.Framework;
using Moq;
using Governer.Internal;
using Governer.Redis;

namespace Governer.Tests
{
	[TestFixture]
	public class GovernerSettingsFixture
	{
		[Test]
		public void GovernerConfigurationTest()
		{
			var clock = new Clock ();
			var storage = new Mock<IGaugeStorage> ();
			Governer
				.Configuration
				.WithClock (clock)
				.WithStorage (storage.Object)
				.Apply ();
			var governer = new Governer (new Gauge ("test", 10), 5);
			Assert.AreEqual (clock, governer.Gauge.Clock);
			Assert.AreSame (storage.Object, governer.Gauge.Storage);
			Governer.Configuration.Reset ();
		}

        [Test]
        public void DefaultWindowExpiryShouldBe1DayTest()
        {
            var defaultExpiry = new TimeSpan(1, 0, 0, 0);
            Mock<IRedisClient> clientMock = new Mock<IRedisClient>();
            TimeSpan timeSpan = TimeSpan.Zero;
            clientMock
                .Setup(x => x.Increment(It.IsAny<string>()))
                .Returns(1);
            clientMock
                .Setup(x => x.Expires(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                .Callback<string, TimeSpan>((s, t) => timeSpan = t);
            var governer = new Governer(new Gauge("some-gauge-name", 10, storage: new GaugeStorage(clientMock.Object)), 5);
            governer.IsAllowed();
            Assert.AreEqual(defaultExpiry, timeSpan);
        }

        [Test]
        public void WindowExpiryShouldBeUsedFromConfigurationTest()
        {
            var expiry = new TimeSpan(0,10,0);
            Governer
                .Configuration
                .WithWindowExpiry(expiry)
                .Apply();
            Mock<IRedisClient> clientMock = new Mock<IRedisClient>();
            TimeSpan timeSpan = TimeSpan.Zero;
            clientMock
                .Setup(x => x.Increment(It.IsAny<string>()))
                .Returns(1);
            clientMock
                .Setup(x => x.Expires(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                .Callback<string, TimeSpan>((s, t) => timeSpan = t);
            var governer = new Governer(new Gauge("some-gauge-name", 10, storage: new GaugeStorage(clientMock.Object)), 5);
            governer.IsAllowed();
            Assert.AreEqual(expiry, timeSpan);
            Governer.Configuration.Reset();
        }
			
	}
}


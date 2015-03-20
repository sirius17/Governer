using System;
using NUnit.Framework;
using Moq;
using Governer.Redis;

namespace Governer.Tests
{
	[TestFixture]
	public class RedisGaugeStorageFixture
	{
		[Test]
		public void IncrementTest ()
		{
			var gaugeName = Guid.NewGuid ().ToString ("N");
			var clientMock = new Mock<IRedisClient> ();
			clientMock.Setup (x => x.Increment (It.IsAny<string> ())).Returns (1);
			var storage = new GaugeStorage (clientMock.Object);
            ulong window = 1;
			Assert.AreEqual (1, storage.Increment (gaugeName, window));
			clientMock.Verify (x => x.Increment (gaugeName + "_1"), Times.Once ());
			clientMock.Verify (x => x.Expires (gaugeName + "_1", It.Is<TimeSpan>( t => t.Seconds == (int) window * 2)), Times.Once ());
		}

        [Test]
        public void IncrementTestWhereCounterIsGreaterThanOne()
        {
            var gaugeName = Guid.NewGuid().ToString("N");
            var clientMock = new Mock<IRedisClient>();
            clientMock.Setup(x => x.Increment(It.IsAny<string>())).Returns(10);
            var storage = new GaugeStorage(clientMock.Object);
            ulong window = 1;
            Assert.AreEqual(10, storage.Increment(gaugeName, window));
            clientMock.Verify(x => x.Increment(gaugeName + "_1"), Times.Once());
            clientMock.Verify(x => x.Expires(It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Never);
        }
	}
}


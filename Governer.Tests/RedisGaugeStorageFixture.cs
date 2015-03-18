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
			Assert.AreEqual (1, storage.Increment (gaugeName, 1));
			clientMock.Verify (x => x.Increment (gaugeName + "_1"), Times.Once ());
			clientMock.Verify (x => x.Expires (gaugeName + "_1", It.Is<TimeSpan>( t => t.Days == 1)), Times.Once ());
		}
	}
}


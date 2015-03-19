using System;
using NUnit.Framework;
using Moq;
using Governer.Internal;

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
			
	}
}


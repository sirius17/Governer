using System;
using NUnit.Framework;

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
		public void GetUtcNowWithOffsetTest()
		{
			var clock = new Clock ();
			var offset = new TimeSpan (0, 0, 10);
			clock.OffSet = offset;
			var currentTime = DateTime.UtcNow;
			var clockTime = clock.UtcNow;
			var timeDifference = clockTime - currentTime;
			Assert.IsTrue (timeDifference.Subtract(offset).Duration().TotalSeconds < 1.0d);
		}


	}
}


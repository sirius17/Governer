using NUnit.Framework;
using System;

namespace Governer.Tests
{
	[TestFixture ()]
	public class TimeServiceFixture
	{
		[Test ()]
		public void InitializeTimeSourceTest ()
		{
			var offset = new TimeSpan (0, 0, 10);
			var timeSource = new TimeService (offset);
			var currentTime = DateTime.UtcNow;
			var timestampFromTimeSource = timeSource.GetCurrentTime (currentTime)
				;
			Assert.AreEqual (10, (timestampFromTimeSource - currentTime).Seconds);
		}
	}
}


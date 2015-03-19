using System;
using NUnit.Framework;

namespace Governer.Tests
{
	[TestFixture]
	public class NtpTimeServerFixture
	{
		[Test]
		public void GetNetworkTimeTest ()
		{
			var server = new NtpTimeServer ("pool.ntp.org");
			var serverNow = server.GetCurrentUtcTime ();
			var localNow = DateTime.UtcNow;
			var difference = Convert.ToInt32(( serverNow - localNow ).Duration().TotalSeconds );
			Console.WriteLine ("Server time: {0} and local time: {1}", serverNow, localNow);
			Assert.IsTrue (difference < 30);
		}
	}
}


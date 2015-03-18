using System;

namespace Governer
{
	public class Governer
	{
		public Governer (string name, int maxRatePerSecond, TimeSpan windowSize)
		{
			var windowSizeInSeconds = Convert.ToInt32(windowSize.TotalSeconds);
			_gauge = new Gauge (name, windowSizeInSeconds);
			_maxCount = windowSizeInSeconds * maxRatePerSecond;
		}

		private readonly Gauge _gauge;
		private readonly long _maxCount = 0;
		
		public bool IsAllowed ()
		{
			var count = _gauge.Increment ();
			return count <= _maxCount;
		}
	}
}


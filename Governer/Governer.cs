using System;

namespace Governer
{
	public class Governer
	{
		public Governer (Gauge gauge, int maxRatePerSecond)
		{
			_gauge = gauge;
			_maxCount = (ulong)(gauge.WindowSizeInSeconds * maxRatePerSecond);
		}

		private readonly Gauge _gauge;
		private readonly ulong _maxCount = 0;
		
		public bool IsAllowed ()
		{
			var count = _gauge.Increment ();
			return count <= _maxCount;
		}
	}
}


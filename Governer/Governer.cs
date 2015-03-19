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

		public static readonly GovernerSettingsBuilder Configuration = new GovernerSettingsBuilder();

		internal static readonly GovernerSettings Settings = new GovernerSettings();

		private readonly Gauge _gauge;
		private readonly ulong _maxCount = 0;
		
		public bool IsAllowed ()
		{
			var count = _gauge.Increment ();
			return count <= _maxCount;

			
		}
	}
}


using System;
using Governer.Internal;

namespace Governer
{
	public class Governer
	{
		public Governer (Gauge gauge, int maxRatePerSecond)
		{
			this.Gauge = gauge;
			_maxCount = (ulong)(gauge.WindowSizeInSeconds * maxRatePerSecond);
		}

		public static readonly GovernerSettingsBuilder Configuration = new GovernerSettingsBuilder();

		internal static readonly GovernerSettings Settings = new GovernerSettings();

		public Gauge Gauge {get; private set;}
		private readonly ulong _maxCount = 0;
		
		public bool IsAllowed ()
		{
			var count = this.Gauge.Increment ();
			return count <= _maxCount;
		}
	}
}


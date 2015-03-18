using System;
using System.Threading;

namespace Governer
{
	public class Gauge
	{
		public Gauge (string name, int windowSizeInSeconds)
		{
			this.Name = name;
			this.WindowSize = windowSizeInSeconds;
			_window = this.GetWindow ();
		}

		public static readonly DateTime EpochTime = new DateTime(2015,1,1, 0,0,0, DateTimeKind.Utc);

		public string Name {
			get;
			private set;
		}

		public int WindowSize {
			get;
			private set;
		}

		private int _count = 0;
		private ulong _window = 0;
		private readonly object _syncRoot = new object();
		public int Increment ()
		{
			if (_window != this.GetWindow ())
				this.RefreshWindow ();
			_count++;
			return _count;
			
		}

		void RefreshWindow ()
		{
			lock (_syncRoot) 
			{
				var window = this.GetWindow ();
				if (window != _window) 
				{
					_window = window;
					_count = 0;
				}
			}
		}

		private ulong GetWindow ()
		{
			var delta = (DateTime.UtcNow - EpochTime).TotalSeconds;
			return Convert.ToUInt64 (delta) / (ulong)this.WindowSize;
		}
	}
}


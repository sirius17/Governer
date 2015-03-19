using System;
using System.Diagnostics;

namespace Governer.Internal
{
	public class TimeService : IDisposable
	{
		public TimeService (ITimeServer[] servers, Timer timer = null)
		{
			this.Servers = servers;
			this.Timer = timer ?? new DateTimeTimer ();
		}

		public ITimeServer[] Servers { get; private set;}

		public Timer Timer {get; set;}

		public TimeSpan GetClockOffset (DateTime utcNow)
		{
			if (this.Servers == null || this.Servers.Length == 0)
				return TimeSpan.Zero;
			for (int i = 0; i < this.Servers.Length; i++) 
			{
				var server = this.Servers [i];
				var now = DateTime.UtcNow;
				DateTime serverNow;
				int networkLatencyInSeconds;
				if (this.TryGetTime (now, server, out serverNow, out networkLatencyInSeconds) == true)
					return this.CalculateOffset (now, serverNow, networkLatencyInSeconds);
			}

			return TimeSpan.Zero;
				
		}

		private bool TryGetTime (DateTime now, ITimeServer server, out DateTime serverNow, out int networkLatencyInSeconds)
		{
			try 
			{
				// Not using stopwatch for latency calculation as we need latency in seconds.
				// For that resolution, DateTime should provide adequate level of precision.
				DateTime serverTime = now;
				networkLatencyInSeconds = this.Timer.MeasureInSeconds( () => 
																		{
																			serverTime = server.GetCurrentUtcTime(now);
																		});
				serverNow = serverTime;
				return true;
			}
			catch
			{
				serverNow = now;
				networkLatencyInSeconds = 0;
				return false;
			}
		}

		TimeSpan CalculateOffset (DateTime now, DateTime serverNow, int networkLatencyInSeconds)
		{
			// Algorithm
			// We have the client and server version of the same time .
			// The delta includes two parts 
			//	- the time difference in clocks
			//	- the network latency
			// So we can use the network latency we have to adjust the time difference to get a fair approximation
			// of the time difference between the client and server.

			var timeDifference = serverNow - now;
			var networkTime = new TimeSpan (0, 0, networkLatencyInSeconds / 2);
			var isNegative = timeDifference.TotalSeconds < 0.0d;
			return isNegative ? 
				timeDifference.Subtract (networkTime.Negate ()) : 
				timeDifference.Subtract (networkTime);
		}

		#region IDisposable implementation

		private bool _isDisposed = false;
		public void Dispose ()
		{
			if (_isDisposed == true)
				return;
			Dispose (true);	
			_isDisposed = true;
		}

		#endregion

		protected void Dispose(bool fromDispose )
		{
			if (fromDispose == false)
				return;
			else 
			{
				GC.SuppressFinalize (this);
				var servers = this.Servers;
				if (servers != null && servers.Length > 0)
					Array.ForEach (servers, s => s.Dispose ());
				this.Servers = null;
			}
		}
	}


}


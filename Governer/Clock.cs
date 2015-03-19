using System;
using Timer2 = System.Threading.Timer;

namespace Governer
{
	public sealed class Clock : IDisposable
	{
		public Clock(TimeService timeService = null, TimeSpan? syncInterval = null, ITimeSource timeSource = null)
		{
			this.TimeSource = timeSource ?? new SystemTimeSource();
			this.TimeService = timeService;
			if (this.TimeService != null) 
			{
				Sync ();
				syncInterval = syncInterval ?? new TimeSpan (0, 5, 0);
				_timer = new Timer2 (x => Sync (), null, syncInterval.Value, syncInterval.Value);
			}

		}

		private static readonly Clock _default = new Clock ();
		public static Clock Default 
		{
			get 
			{
				return _default;
			}
		}

		public ITimeSource TimeSource {get; private set;}

		public TimeService TimeService  {get; private set;}

		public TimeSpan OffSet {get; private set;}

		private Timer2 _timer = null;

		private void Sync()
		{
			this.OffSet = this.TimeService.GetClockOffset (this.GetSystemUtcTime ());
		}

		public DateTime UtcNow 
		{
			get 
			{
				return this.GetSystemUtcTime().Add (this.OffSet);
			}	
		}

		public DateTime GetSystemUtcTime ()
		{
			return this.TimeSource.GetUtcNow ();
		}

		#region IDisposable implementation

		bool _isDisposed = false;
		public void Dispose ()
		{
			if (_isDisposed == true)
				return;
			var timeService = this.TimeService;
			if (timeService != null) 
			{
				timeService.Dispose ();
				this.TimeService = null;
			}
			var timer = _timer;
			if (timer != null)
				timer.Dispose ();
			_timer = null;
			_isDisposed = true;
		}

		#endregion
	}
}
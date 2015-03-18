using System;
using System.Threading;

namespace Governer
{
	public class Gauge
	{	
		public Gauge (string name, int windowSizeInSeconds, IGaugeStorage storage = null)
		{
			this.Name = name;
			this.WindowSize = windowSizeInSeconds;
			_storage = storage ?? InProcGaugeStorage.Instance;
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

		private IGaugeStorage _storage;

		public ulong Increment ()
		{
			var window = this.GetWindow ();
			return _storage.Increment (this.Name, window);
			
		}

		private ulong GetWindow ()
		{
			var delta = (DateTime.UtcNow - EpochTime).TotalSeconds;
			return Convert.ToUInt64 (delta) / (ulong)this.WindowSize;
		}
	}
}


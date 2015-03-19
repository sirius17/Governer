using System;
using System.Threading;

namespace Governer
{
	public class Gauge
	{	
		public Gauge (string name, int windowSizeInSeconds, Clock clock = null, IGaugeStorage storage = null)
		{
			this.Name = name;
			this.WindowSizeInSeconds = windowSizeInSeconds;
			if (storage == null) 
			{
				if (Governer.Settings.StorageFactory == null)
					_storage = InProcGaugeStorage.Instance;
				else
					_storage = Governer.Settings.StorageFactory();
			}
			this.Clock = clock ?? (Governer.Settings.Clock ?? Clock.Default);
		}

		public static readonly DateTime EpochTime = new DateTime(2015,1,1, 0,0,0, DateTimeKind.Utc);

		public string Name {get; private set;}

		public int WindowSizeInSeconds {get; private set;}

		public Clock Clock {get; private set;}

		private IGaugeStorage _storage;

		public ulong Increment ()
		{
			var window = this.GetWindow ();
			try 
			{
				return _storage.Increment (this.Name, window);
			}
			catch 
			{
				return 0;
			}
			
		}

		private ulong GetWindow ()
		{
			var delta = (this.Clock.UtcNow - EpochTime).TotalSeconds;
			return Convert.ToUInt64 (delta) / (ulong)this.WindowSizeInSeconds;
		}
	}
}


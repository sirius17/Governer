using System;
using System.Threading;
using Governer.Internal;

namespace Governer
{
	public class Gauge
	{	
		public Gauge (string name, int windowSizeInSeconds, Clock clock = null, IGaugeStorage storage = null)
		{
			this.Name = name;
			this.WindowSizeInSeconds = windowSizeInSeconds;

			if (storage != null) 
				this.Storage = storage;
			else 
			{
				if (Governer.Settings.StorageFactory == null)
					this.Storage = InProcGaugeStorage.Instance;
				else
					this.Storage = Governer.Settings.StorageFactory();
			}
			this.Clock = clock ?? (Governer.Settings.Clock ?? Clock.Default);
		}

		public static readonly DateTime EpochTime = new DateTime(2015,1,1, 0,0,0, DateTimeKind.Utc);

		public string Name {get; private set;}

		public int WindowSizeInSeconds {get; private set;}

		public Clock Clock {get; private set;}

		public IGaugeStorage Storage {get; private set;}

		public ulong Increment ()
		{
			var window = this.GetWindow ();
			try 
			{
				return this.Storage.Increment (this.Name, window);
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


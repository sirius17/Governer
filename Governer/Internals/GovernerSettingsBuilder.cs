using System;
using System.Collections.Generic;

namespace Governer
{
	public class GovernerSettingsBuilder
	{
		private List<Action<GovernerSettings>> _actions = new List<Action<GovernerSettings>>();

		public GovernerSettingsBuilder WithClock( ITimeServer timeServer, TimeSpan? syncInterval = null )
		{
			return this.WithClock (new [] { timeServer }, syncInterval);
		}

		public GovernerSettingsBuilder WithClock( ITimeServer[] timeServers, TimeSpan? syncInterval = null )
		{
			Action<GovernerSettings> action = s => 
			{
				s.Clock = new Clock( new TimeService(timeServers), syncInterval);
			};
			_actions.Add (action);
			return this;
		}

		public GovernerSettingsBuilder WithClock( Clock clock )
		{
			Action<GovernerSettings> action = s => 
			{
				s.Clock = clock;
			};
			_actions.Add (action);
			return this;
		}

		public GovernerSettingsBuilder WithStorage( string redisConnectionString )
		{
			return this.WithStorage (new Redis.GaugeStorage (redisConnectionString));
		}

		public GovernerSettingsBuilder WithStorage( IGaugeStorage gaugeStorage )
		{
			_actions.Add ( s => s.StorageFactory = () => gaugeStorage );
			return this;
		}

		public GovernerSettingsBuilder WithStorage( Func<IGaugeStorage> buildStorage )
		{
			_actions.Add ( s => s.StorageFactory = buildStorage );
			return this;
		}

		public void Apply()
		{
			_actions.ForEach (x => x (Governer.Settings));
			SetupDefaults (Governer.Settings);
		}

		private void SetupDefaults (GovernerSettings settings)
		{
			if (settings.StorageFactory == null)
				settings.StorageFactory = () => InProcGaugeStorage.Instance;
			if (settings.Clock == null)
				settings.Clock = new Clock ();
		}
	}
}


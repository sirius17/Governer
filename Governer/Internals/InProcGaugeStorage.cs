using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Governer.Internal
{
	public class InProcGaugeStorage : IGaugeStorage
	{
		private static readonly InProcGaugeStorage _instance = new InProcGaugeStorage();
		public static InProcGaugeStorage Instance 
		{
			get{ return _instance; }
		}

		private InProcGaugeStorage() 
		{
		}

		private ConcurrentDictionary<string, GaugeWindow> _map = 
			new ConcurrentDictionary<string, GaugeWindow>(StringComparer.InvariantCultureIgnoreCase);

		#region IGaugeStorage implementation
		public ulong Increment (string gaugeName, ulong window)
		{
			var gaugeWindow = GetExistingValue (gaugeName, window);
			if (gaugeWindow.Matches(window) == false ) 
				gaugeWindow = RefreshGaugeValue (gaugeName, window);
			return gaugeWindow.Increment ();
		}
		#endregion

		private GaugeWindow GetExistingValue (string gaugeName, ulong window)
		{
			GaugeWindow gaugeValue = null;
			if (_map.TryGetValue (gaugeName, out gaugeValue) == false) 
			{
				gaugeValue = new GaugeWindow (window);
				_map [gaugeName] = gaugeValue;
			}
			return gaugeValue;
		}

		private readonly object _syncRoot = new object();

		private GaugeWindow RefreshGaugeValue (string gaugeName, ulong window)
		{
			GaugeWindow value = null;
			lock (_syncRoot) 
			{
				value = this.GetExistingValue(gaugeName, window);
				if (value.Window != window) 
				{
					value = new GaugeWindow (window);
					_map [gaugeName] = value;
				}
			}
			return value;
		}
	}

	internal class GaugeWindow
	{
		public GaugeWindow(ulong window)
		{
			this.Window = window;
			this.Count = 0;
		}

		public ulong Window {get; private set;}

		public ulong Count {get; private set;}

		public ulong Increment()
		{
			return ++this.Count;
		}

		public bool Matches (ulong window)
		{
			return this.Window.Equals (window);
		}
	}
}


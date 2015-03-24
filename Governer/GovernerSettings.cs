using System;
using System.Collections.Generic;
using Governer.Internal;

namespace Governer
{
	internal class GovernerSettings
	{
        public GovernerSettings()
        {
            this.WindowExpiry = DefaultWindowExpiry;
        }

        public static readonly TimeSpan DefaultWindowExpiry = new TimeSpan(1, 0, 0, 0);

		public Func<IGaugeStorage> StorageFactory { get; set; }

		public Clock Clock {get; set;}

        public TimeSpan WindowExpiry { get; set; }
    }
}


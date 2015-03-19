using System;
using System.Collections.Generic;

namespace Governer
{
	internal class GovernerSettings
	{
		public Func<IGaugeStorage> StorageFactory { get; set; }

		public Clock Clock {get; set;}
	}
}


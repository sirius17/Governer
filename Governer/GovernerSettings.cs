using System;
using System.Collections.Generic;
using Governer.Internal;

namespace Governer
{
	internal class GovernerSettings
	{
		public Func<IGaugeStorage> StorageFactory { get; set; }

		public Clock Clock {get; set;}
	}
}


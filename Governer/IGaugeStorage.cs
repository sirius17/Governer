using System;

namespace Governer
{
	public interface IGaugeStorage
	{
		ulong Increment(string gaugeName, ulong window);
	}
}


using System;

namespace Governer.Internal
{
	public interface IGaugeStorage
	{
		ulong Increment(string gaugeName, ulong window);
	}
}


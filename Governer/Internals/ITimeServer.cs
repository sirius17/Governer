using System;

namespace Governer.Internal
{
	public interface ITimeServer : IDisposable
	{
		DateTime GetCurrentUtcTime();
	}
}


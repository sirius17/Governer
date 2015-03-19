using System;

namespace Governer
{
	public interface ITimeServer : IDisposable
	{
		DateTime GetCurrentUtcTime(DateTime utcNow );
	}
}


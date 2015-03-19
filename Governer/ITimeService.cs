using System;

namespace Governer
{
	public interface ITimeServer
	{
		DateTime GetCurrentUtcTime(DateTime utcNow );
	}
}


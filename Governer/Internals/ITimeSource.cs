using System;

namespace Governer
{
	public interface ITimeSource
	{
		DateTime GetUtcNow();
	}
}


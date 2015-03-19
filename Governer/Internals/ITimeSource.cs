using System;

namespace Governer.Internal
{
	public interface ITimeSource
	{
		DateTime GetUtcNow();
	}
}


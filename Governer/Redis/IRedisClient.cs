using System;

namespace Governer
{
	public interface IRedisClient
	{
		long Increment( string key );

		void Expires( string key, TimeSpan timespan );
	}
}


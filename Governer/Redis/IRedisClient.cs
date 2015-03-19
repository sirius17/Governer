using System;

namespace Governer.Redis
{
	public interface IRedisClient
	{
		long Increment( string key );

		void Expires( string key, TimeSpan timespan );
	}
}


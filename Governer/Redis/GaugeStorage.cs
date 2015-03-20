using System;
using StackExchange.Redis;
using Governer.Internal;

namespace Governer.Redis
{
	public class GaugeStorage : IGaugeStorage
	{
		public GaugeStorage(string connString) : this( new RedisClient(connString) )
		{
		}

		public GaugeStorage( IRedisClient redisClient )
		{
			_client = redisClient;
		}

		private readonly IRedisClient _client;

		#region IGaugeStorage implementation
        public ulong Increment(string gaugeName, ulong window)
        {
            var key = string.Format("{0}_{1}", gaugeName, window);
            var value = _client.Increment(key);
            if (value == 1)
                _client.Expires(key, new TimeSpan(0, 0, 0, (int)window * 2));
            return (ulong)value;
        }
		#endregion
				
	}
}


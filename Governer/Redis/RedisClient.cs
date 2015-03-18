using System;
using System.Collections.Concurrent;
using StackExchange.Redis;

namespace Governer
{
	public class RedisClient : IRedisClient
	{
		public RedisClient(string connString)
		{
			_multiplexer = MultiplexerFactory.Create (connString);
		}

		private readonly ConnectionMultiplexer _multiplexer;

		#region IRedisClient implementation
		public long Increment (string key)
		{
			var db = _multiplexer.GetDatabase ();
			return db.StringIncrement (key);
		}

		public void Expires (string key, TimeSpan timespan)
		{
			var db = _multiplexer.GetDatabase ();
			db.KeyExpire (key, timespan);
		}
		#endregion
		
	}

	internal static class MultiplexerFactory
	{
		private static ConcurrentDictionary<string, ConnectionMultiplexer> _cache = new ConcurrentDictionary<string, ConnectionMultiplexer>();

		public static ConnectionMultiplexer Create(string connString)
		{
			ConnectionMultiplexer multiplexer = null;
			if (_cache.TryGetValue (connString, out multiplexer) == false) 
			{
				multiplexer = ConnectionMultiplexer.Connect (connString);
				_cache [connString] = multiplexer;
			}
			return multiplexer;
		}
	}
}


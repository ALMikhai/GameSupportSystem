using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Server.Services.Redis
{
	/// <summary>
	/// Implementation of redis cahce interface.
	/// </summary>
	public class RedisCacheService : IRedisCacheService
	{
		private readonly IDistributedCache cache;

		/// <summary>
		/// Implementation of redis cahce interface.
		/// </summary>
		/// <param name="cache">Distributed cache of serialized values.</param>
		public RedisCacheService(IDistributedCache cache) {
			this.cache = cache;
		}

		/// <summary>
		/// Get value from redis.
		/// </summary>
		/// <typeparam name="T">Type of data.</typeparam>
		/// <param name="key">Key of data.</param>
		/// <returns>Value of type T.</returns>
		public T Get<T>(string key) {
			var value = cache.GetString(key);
			if (value != null) {
				return JsonSerializer.Deserialize<T>(value);
			}
			return default;
		}

		/// <summary>
		/// Set value to redis.
		/// </summary>
		/// <typeparam name="T">Type of data.</typeparam>
		/// <param name="key">Key of data.</param>
		/// <param name="value">Value.</param>
		/// <returns>Seted value of type T.</returns>
		public T Set<T>(string key, T value) {
			var timeOut = new DistributedCacheEntryOptions {
				AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
				SlidingExpiration = TimeSpan.FromMinutes(60)
			};
			cache.SetString(key, JsonSerializer.Serialize(value), timeOut);
			return value;
		}	
	}
}

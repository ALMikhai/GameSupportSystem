namespace Server.Services.Redis
{
	/// <summary>
	/// Redis cahce interface.
	/// </summary>
	public interface IRedisCacheService
	{
		/// <summary>
		/// Get value from redis.
		/// </summary>
		/// <typeparam name="T">Type of data.</typeparam>
		/// <param name="key">Key of data.</param>
		/// <returns>Value of type T.</returns>
		T Get<T>(string key);

		/// <summary>
		/// Set value to redis.
		/// </summary>
		/// <typeparam name="T">Type of data.</typeparam>
		/// <param name="key">Key of data.</param>
		/// <param name="value">Value.</param>
		/// <returns>Seted value of type T.</returns>
		T Set<T>(string key, T value);
	}
}

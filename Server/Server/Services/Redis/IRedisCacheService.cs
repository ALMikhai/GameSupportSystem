namespace Server.Services.Redis
{
	public interface IRedisCacheService
	{
		T Get<T>(string key);
		T Set<T>(string key, T value);
	}
}

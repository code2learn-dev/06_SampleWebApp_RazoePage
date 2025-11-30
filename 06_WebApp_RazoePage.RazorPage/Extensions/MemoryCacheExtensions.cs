using _06_WebApp_RazoePage.RazorPage.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Extensions
{
	public static class MemoryCacheExtensions
	{ 
		public static void SetMemoryCache<TEntity>(this IMemoryCache cache, string? key, TEntity? value) where TEntity : BaseViewModel
		{
			if (string.IsNullOrEmpty(key) || value is null) return;

			string serializedValue = JsonConvert.SerializeObject(value);
			cache.Set(key, serializedValue);
		}

		public static TEntity? GetMemoryCacheValue<TEntity>(this IMemoryCache cache, string? key)
		{
			if (string.IsNullOrWhiteSpace(key)) return default;

			string? serializedData = cache.Get<string>(key);
			if (string.IsNullOrEmpty(serializedData)) return default;

			var entity = JsonConvert.DeserializeObject<TEntity>(serializedData);
			return entity;
		}
	}
}

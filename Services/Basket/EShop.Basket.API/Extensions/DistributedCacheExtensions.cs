using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EShop.Basket.API.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
                    string recordId,
                    T data,
                    TimeSpan? absoluteExpireTime = null,
                    TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions();

        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
        options.SlidingExpiration = unusedExpireTime;

        var jsonData = JsonConvert.SerializeObject(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }
    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);
        if (jsonData is null)
            return default(T);

        return JsonConvert.DeserializeObject<T>(jsonData);
            
    }
}

using System;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CommonLib.Caching
{
    public static class CachingHelperExtensions
    {

        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = unusedExpireTime ?? TimeSpan.FromSeconds(30);

            var jsonData = JsonConvert.SerializeObject(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }


        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId) where T: Document
        {
            var key =  recordId;
            var jsonString = await cache.GetStringAsync(key);
            if (jsonString == null)
            {
                return default(T);
            }
            var json = JsonConvert.DeserializeObject<T>(jsonString);
            json.ReadFrom = "Redis";
            return json;
            //return jsonData == null ? default(T) : JsonConvert.DeserializeObject<T>(jsonData);
        }
        
    }
}
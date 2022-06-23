using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Extensions
{
    public static class DistributedCachExtensions
    {
        /// <summary>
        /// Set an item to the distributed cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <param name="data"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <param name="unusedExpireTime"></param>
        /// <returns></returns>
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            DistributedCacheEntryOptions options = new()
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(10),
                SlidingExpiration = unusedExpireTime
            };

            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            try
            {
                await cache.SetStringAsync(recordId, jsonData, options);
            }
            catch (Exception ex)
            {
                ApplicationLogger.Logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get an item from the distributed cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            try
            {
                var jsonData = await cache.GetStringAsync(recordId);

                if (jsonData is null)
                {
                    return default(T);
                }

                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            catch (Exception ex)
            {
                ApplicationLogger.Logger.LogError(ex, ex.Message);

                return default(T);
            }
        }
    }
}

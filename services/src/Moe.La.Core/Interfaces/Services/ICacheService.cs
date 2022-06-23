using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ICacheService
    {
        Task<ReturnResult<string>> GetCacheValueAsync(string key);

        Task<ReturnResult<string>> SetCachValueAsync(string key, string value);
    }
}

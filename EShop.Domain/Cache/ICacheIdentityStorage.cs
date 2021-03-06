using System.Threading.Tasks;
using EShop.Domain.Identity;
using EShop.Domain.Identity.JWT;

namespace EShop.Domain.Cache;

public interface ICacheIdentityStorage
{
    Task<RefreshToken> GetCacheValueAsync(string key);

    Task SetCacheValueAsync(string key, RefreshToken value);
}
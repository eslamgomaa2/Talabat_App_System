using Microsoft.Extensions.Caching.Memory;

namespace Domin.Helper
{
    public class MemoryCashEntryopt
    {

        public MemoryCacheEntryOptions Default =>

            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Priority = CacheItemPriority.High,
            };
    }
}

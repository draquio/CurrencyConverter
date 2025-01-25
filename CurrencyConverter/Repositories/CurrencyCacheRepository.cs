using CurrencyConverter.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CurrencyConverter.Repositories
{
    public class CurrencyCacheRepository : ICurrencyCacheRepository
    {

        private readonly IDistributedCache _cache;

        public CurrencyCacheRepository(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task SetRate(string key, decimal rate, TimeSpan expiry)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiry,
                };
                await _cache.SetStringAsync(key, rate.ToString(), options);
            }
            catch
            {
                throw;
            }
        }
        public async Task<decimal?> GetRate(string key)
        {
            try
            {
                var response = await _cache.GetStringAsync(key);
                if(decimal.TryParse(response, out var rate)) return rate;
                return null;
            }
            catch
            {
                throw;
            }
        }


    }
}

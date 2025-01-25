namespace CurrencyConverter.Repositories.Interfaces
{
    public interface ICurrencyCacheRepository
    {
        Task SetRate(string key, decimal rate, TimeSpan expiry);
        Task<decimal?> GetRate(string key);
    }
}

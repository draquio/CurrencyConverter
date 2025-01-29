using CurrencyConverter.Entities;

namespace CurrencyConverter.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task AddLog(ConversionLog log);
        Task<List<TopConversionReport>> GetTopConversions(int Ntop, string range);
    }
}

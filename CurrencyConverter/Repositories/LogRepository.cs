using CurrencyConverter.Context;
using CurrencyConverter.Entities;
using CurrencyConverter.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly AppDBContext _dbContext;

        public LogRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddLog(ConversionLog log)
        {
            try
            {
                _dbContext.Set<ConversionLog>().Add(log);
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ConversionLog>> GetTopConversions(int Ntop)
        {
            try
            {
                List<ConversionLog> conversionLogs = await _dbContext.Set<ConversionLog>()
                    .GroupBy(log => new { log.FromCurrency, log.ToCurrency })
                    .Select(group => new ConversionLog
                    {
                        FromCurrency = group.Key.FromCurrency,
                        ToCurrency = group.Key.ToCurrency,
                        Amount = group.Sum(x => x.Amount),
                        ConvertedAmount = group.Sum(x => x.ConvertedAmount),
                        Timestamp = group.Max(x => x.Timestamp),
                    })
                    .OrderByDescending(log => log.Amount)
                    .Take(Ntop)
                    .ToListAsync();
                return conversionLogs;
            }
            catch
            {
                throw;
            }
        }
    }
}

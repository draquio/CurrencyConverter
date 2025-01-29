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

        public async Task<List<TopConversionReport>> GetTopConversions(int Ntop, string range)
        {
            try
            {
                DateTime start = range switch
                {
                    "30d" => DateTime.Now.AddDays(-30),
                    _ => DateTime.Now.AddDays(-7),
                };
               List<TopConversionReport> topConversionReport = await _dbContext.Set<ConversionLog>()
                    .Where(log => log.Timestamp >= start)
                    .GroupBy(log => new { log.FromCurrency, log.ToCurrency })
                    .Select(group => new TopConversionReport
                    {
                        FromCurrency = group.Key.FromCurrency,
                        ToCurrency = group.Key.ToCurrency,
                        ConversionCount = group.Count(),
                        TotalAmount = group.Sum(x => x.Amount),
                        Range = range
                    })
                    .OrderByDescending(report => report.ConversionCount)
                    .Take(Ntop)
                    .ToListAsync();
                return topConversionReport;
            }
            catch
            {
                throw;
            }
        }
    }
}

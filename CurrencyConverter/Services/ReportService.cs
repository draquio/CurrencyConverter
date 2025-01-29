using AutoMapper;
using CurrencyConverter.DTOs;
using CurrencyConverter.Repositories.Interfaces;
using CurrencyConverter.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace CurrencyConverter.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public ReportService(ILogRepository logRepository, IMapper mapper, IDistributedCache cache)
        {
            _logRepository = logRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public string GenerateCsv(List<TopConversionReportDTO> report)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("FromCurrency,ToCurrency,ConversionCount,TotalAmount,Range");
                foreach(var item in report)
                {
                    sb.AppendLine($"{item.FromCurrency},{item.ToCurrency},{item.ConversionCount},{item.TotalAmount.ToString("0.##")},{item.Range}");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Un error ocurrió: {ex.Message}", ex);
            }
        }

        public async Task<List<TopConversionReportDTO>> GetTopConversions(int topN, string range)
        {
            try
            {
                string cacheKey = $"TopConversions_{topN}_{range}";
                var cacheData = await _cache.GetStringAsync(cacheKey);
                if(!string.IsNullOrEmpty(cacheData)) return JsonSerializer.Deserialize<List<TopConversionReportDTO>>(cacheData);

                var topConversions = await _logRepository.GetTopConversions(topN, range);
                if (topConversions == null) return new List<TopConversionReportDTO>();

                List<TopConversionReportDTO> TopConversionDTO = _mapper.Map<List<TopConversionReportDTO>>(topConversions);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(TopConversionDTO), cacheOptions);
                return TopConversionDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Un error ocurrió: {ex.Message}", ex);
            }
        }
    }
}

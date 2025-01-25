using AutoMapper;
using CurrencyConverter.Contants;
using CurrencyConverter.DTOs;
using CurrencyConverter.Entities;
using CurrencyConverter.Repositories;
using CurrencyConverter.Repositories.Interfaces;
using CurrencyConverter.Services.Interfaces;

namespace CurrencyConverter.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyCacheRepository _cacheRepository;
        private readonly ILogRepository _logRepository;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public CurrencyService(ICurrencyCacheRepository cacheRepository, ILogRepository logRepository, HttpClient httpClient, IMapper mapper)
        {
            _cacheRepository = cacheRepository;
            _logRepository = logRepository;
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<ConversionResponseDTO> ConvertCurrency(ConversionRequestDTO request)
        {

            try
            {
                var cacheKey = $"{request.FromCurrency}_{request.ToCurrency}";
                var cacheRate = await _cacheRepository.GetRate(cacheKey);

                if (!cacheRate.HasValue)
                {
                    cacheRate = await GetRateFromApi(request.FromCurrency, request.ToCurrency);
                    if(!cacheRate.HasValue) throw new Exception($"Rate no disponible para {request.FromCurrency} a {request.ToCurrency}");
                    await _cacheRepository.SetRate(cacheKey, cacheRate.Value, TimeSpan.FromHours(24));
                }
                var convertedAmount = request.Amount * cacheRate.Value;
                var log = _mapper.Map<ConversionLog>(request);
                log.ConvertedAmount = convertedAmount;
                log.Timestamp = DateTime.UtcNow;

                await _logRepository.AddLog(log);
                ConversionResponseDTO response = _mapper.Map<ConversionResponseDTO>(log);
                response.Rate = cacheRate.Value;
                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Un error ocurrió: {ex.Message}", ex);
            }
        }

        private async Task<decimal?> GetRateFromApi( string fromCurrency, string toCurrency)
        {
            try
            {
                var rates = await FetchRatesFromApi(fromCurrency);
                if (rates.TryGetValue(toCurrency, out var directRates)) return directRates;
                return null;
            }
            catch (Exception ex) 
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private async Task<Dictionary<string,decimal>> FetchRatesFromApi(string baseCurrency)
        {
            try
            {
                var url = $"https://api.exchangerate-api.com/v4/latest/{baseCurrency}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<ExchangeRateApiResponse>();
                return data.Rates;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public List<Currency> GetAllCurrencies()
        {
            try
            {
                List<Currency> currencies = CurrienciesConstant.Currencies;
                return currencies;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}

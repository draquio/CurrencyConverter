using CurrencyConverter.DTOs;
using CurrencyConverter.Entities;

namespace CurrencyConverter.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<ConversionResponseDTO> ConvertCurrency(ConversionRequestDTO request);
        List<Currency> GetAllCurrencies();

    }
}

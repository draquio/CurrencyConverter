using CurrencyConverter.DTOs;

namespace CurrencyConverter.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<ConversionResponseDTO> ConvertCurrency(ConversionRequestDTO request);

    }
}

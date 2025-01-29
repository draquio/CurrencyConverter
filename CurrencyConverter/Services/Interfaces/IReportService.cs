using CurrencyConverter.DTOs;

namespace CurrencyConverter.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<TopConversionReportDTO>> GetTopConversions(int topN, string range);
        string GenerateCsv(List<TopConversionReportDTO> report);
    }
}

namespace CurrencyConverter.DTOs
{
    public class ConversionResponseDTO
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Rate { get; set; }
        public decimal ConvertedAmount { get; set; }
    }
}

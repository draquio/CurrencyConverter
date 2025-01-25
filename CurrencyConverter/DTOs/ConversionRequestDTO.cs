namespace CurrencyConverter.DTOs
{
    public class ConversionRequestDTO
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}

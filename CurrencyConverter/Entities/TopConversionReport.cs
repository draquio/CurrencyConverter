namespace CurrencyConverter.Entities
{
    public class TopConversionReport
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public int ConversionCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Range { get; set; }
    }
}

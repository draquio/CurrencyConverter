namespace CurrencyConverter.DTOs
{
    public class ExchangeRateApiResponse
    {
        public string Base {  get; set; }
        public string Date { get; set; }
        public long TimeLastUpdate { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}

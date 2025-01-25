using CurrencyConverter.DTOs;
using CurrencyConverter.Entities;
using CurrencyConverter.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyConverterController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpPost("convert")]
        public async Task<ActionResult<ConversionResponseDTO>> ConvertCurrency([FromBody] ConversionRequestDTO request)
        {
            try
            {
                var result = await _currencyService.ConvertCurrency(request);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("getCurrencies")]
        public async Task<ActionResult<List<Currency>>> GetCurrencies()
        {
            try
            {
                var result = _currencyService.GetAllCurrencies();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

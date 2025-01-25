using CurrencyConverter.DTOs;
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
    }
}

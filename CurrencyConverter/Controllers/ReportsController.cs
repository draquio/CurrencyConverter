using CurrencyConverter.DTOs;
using CurrencyConverter.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("top-conversion")]
        public async Task<ActionResult<List<TopConversionReportDTO>>> GetTopConversion([FromQuery] int topN = 10, [FromQuery] string range = "7d")
        {
            try
            {
                var report = await _reportService.GetTopConversions(topN, range);
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("export-report")]
        public async Task<IActionResult> ExportReport([FromQuery] int topN = 10, [FromQuery] string range = "7d")
        {
            try
            {
                var report = await _reportService.GetTopConversions(topN, range);
                var csv = _reportService.GenerateCsv(report);
                return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "top-conversion.csv");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

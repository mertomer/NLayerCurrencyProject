using Microsoft.AspNetCore.Mvc;
using NLayerCore.Interfaces;

namespace NLayerDovizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;

        public CurrencyController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("{currencyCode}")]
        public async Task<IActionResult> GetCurrencyRate(string currencyCode)
        {
            var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(currencyCode);
            if (exchangeRate == null)
            {
                return NotFound();
            }
            return Ok(exchangeRate);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using NLayerCore.Entities;
using NLayerCore.Interfaces;
using NLayerInfrastructure.Services;
using System.Threading.Tasks;

namespace NLayerDovizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IExchangeRateService _exchangeRateService;

        public CurrencyController(RedisService redisService, IExchangeRateService exchangeRateService)
        {
            _redisService = redisService;
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

        [HttpPost]
        public async Task<IActionResult> PostCurrencyRate([FromBody] ExchangeRate exchangeRate)
        {
            var key = exchangeRate.CurrencyCode;
            var value = $"{exchangeRate.BanknoteBuying}:{exchangeRate.BanknoteSelling}";

            await _redisService.SetExchangeRateAsync(key, value);

            return CreatedAtAction(nameof(GetCurrencyRate), new { currencyCode = key }, exchangeRate);
        }

        [HttpPut("{currencyCode}")]
        public async Task<IActionResult> PutCurrencyRate(string currencyCode, [FromBody] ExchangeRate exchangeRate)
        {
            if (currencyCode != exchangeRate.CurrencyCode)
            {
                return BadRequest("Currency code mismatch.");
            }

            var key = currencyCode;
            var value = $"{exchangeRate.BanknoteBuying}:{exchangeRate.BanknoteSelling}";

            var existingRate = await _redisService.GetExchangeRateAsync(key);
            if (existingRate == null)
            {
                return NotFound("Currency not found.");
            }

            await _redisService.SetExchangeRateAsync(key, value);

            return NoContent(); 
        }

        [HttpDelete("{currencyCode}")]
        public async Task<IActionResult> DeleteCurrencyRate(string currencyCode)
        {
            var key = currencyCode;

            var existingRate = await _redisService.GetExchangeRateAsync(key);
            if (existingRate == null)
            {
                return NotFound("Currency not found.");
            }

            await _redisService.DeleteExchangeRateAsync(key);

            return NoContent(); 
        }
    }
}
